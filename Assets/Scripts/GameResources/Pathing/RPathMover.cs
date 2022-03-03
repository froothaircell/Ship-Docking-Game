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
        
        private void OnEnable()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _shipSpeed = GetComponent<ShipController>()._shipData.ShipSpeed;
            if (_shipSpeed == null)
            {
                _shipSpeed = 3f;
            }
            GetComponent<RPathingManager>().OnNewPointAdded += SetPoints;
            onPathingStopped += OnPathingStopped;
            _forwardDirection = transform.forward;
            if (_pathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
            }
            _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(MoveStraight());
        }

        private void OnDisable()
        {
            _navMeshAgent.ResetPath();
            onPathingStopped -= OnPathingStopped;
            if (_pathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
            }
        }

        private void OnPathingStopped()
        {
            _forwardDirection = transform.forward;
            ClearPath();
            _pathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(MoveStraight());
        }

        public void ClearPath()
        {
            _navMeshAgent.ResetPath();
            _pathPoints.Clear();
            if (_pathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref _pathingCoroutine);
            }
        }

        private void SetPoints(Vector3 point)
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
                    Debug.Log(pathPoint);
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