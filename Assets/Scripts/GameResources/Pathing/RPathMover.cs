using System;
using System.Collections;
using System.Collections.Generic;
using CoreResources.Utils.Jobs;
using GameResources.Ship;
using UnityEngine;
using UnityEngine.AI;

namespace GameResources.Pathing
{
    public class RPathMover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private List<Vector3> _pathPoints = new List<Vector3>();
        public float distanceThreshold = 0.5f;
        private UpdateJob _pathingCoroutine;
        private float _shipSpeed;
        private Vector3 _forwardDirection;
        
        public Action onPathingStopped = delegate {  };
        
        public void InitPathMover(float speed)
        {
            if(_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();
            SetSpeed(speed);
            onPathingStopped += OnPathingStopped;
            _forwardDirection = transform.forward;
            if (_pathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
            }
            _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(MoveStraight());
        }

        public void ResetPathMover()
        {
            ClearPath();
            onPathingStopped -= OnPathingStopped;
        }

        private void OnPathingStopped()
        {
            _forwardDirection = transform.forward;
            ClearPath();
            _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(MoveStraight());
        }

        private void SetSpeed(float speed)
        {
            _shipSpeed = speed;
            _navMeshAgent.speed = speed;
        }

        private void ClearPath()
        {
            if(_navMeshAgent.isOnNavMesh)
                _navMeshAgent.ResetPath();
            _pathPoints.Clear();
            if (_pathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
            }
        }

        public void SetPoints(Vector3 point)
        {
            _pathPoints.Add(point);
            if (_pathingCoroutine == null)
            {
                _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(UpdatePathing());
            }
            else
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
                _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(UpdatePathing());
            }
        }

        private IEnumerator UpdatePathing()
        {
            while (true)
            {
                if (ShouldSetDestination())
                {
                    Vector3 pathPoint = _pathPoints[0];
                    _pathPoints.RemoveAt(0);
                    _navMeshAgent.SetDestination(pathPoint);
                    yield return 0;
                }
                else
                {
                    if (_pathPoints.Count == 0)
                    {
                        onPathingStopped.Invoke();
                    }
                    yield return 0;
                }
            }
        }

        private IEnumerator MoveStraight()
        {
            while (true)
            {
                transform.position += _forwardDirection * Time.deltaTime * _shipSpeed;
                yield return 0;
            }
        }

        private bool ShouldSetDestination()
        {
            if (_pathPoints.Count == 0)
                return false;

            if (_navMeshAgent.hasPath == false || _navMeshAgent.remainingDistance < distanceThreshold)
                return true;

            return false;
        }
    }
}