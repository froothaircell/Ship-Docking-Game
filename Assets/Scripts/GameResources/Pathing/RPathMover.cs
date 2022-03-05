using System;
using System.Collections;
using System.Collections.Generic;
using CoreResources.Pool;
using CoreResources.Utils.Jobs;
using UnityEngine;
using UnityEngine.AI;

namespace GameResources.Pathing
{
    public class RPathMover : MonoBehaviour
    { 
        delegate void ActionRef<T>(ref T arg);
        
        private NavMeshAgent _navMeshAgent;

        // private List<Vector3> _pathPoints = new List<Vector3>();
        public float distanceThreshold = 0.5f;
        private float _shipSpeed;
        private Vector3 _forwardDirection;

        public Action onPathingStopped = delegate { };
        private ActionRef<PooledList<Vector3>> PathingAction;
        
        public void InitPathMover(float speed)
        {
            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();
            SetSpeed(speed);
            onPathingStopped += OnPathingStopped;
            _forwardDirection = transform.forward;
        }

        public void ResetPathMover()
        {
            ClearPath();
            onPathingStopped -= OnPathingStopped;
        }

        private void SetSpeed(float speed)
        {
            _shipSpeed = speed;
            _navMeshAgent.speed = speed;
        }
        
        private void OnPathingStopped()
        {
            _forwardDirection = transform.forward;
            ClearPath();
        }
        
        private void ClearPath()
        {
            if (_navMeshAgent.isOnNavMesh)
                _navMeshAgent.ResetPath();
        }

        public void MoveAgent(ref PooledList<Vector3> pathPoints)
        {
            if (PathingAction != null)
            {
                PathingAction.Invoke(ref pathPoints);
            }
            else
            {
                PathingAction = MoveStraight;
            }
        }

        private void MoveOnPath(ref PooledList<Vector3> pathPoints)
        {
            if (pathPoints.Count > 0)
            {
                if (ShouldSetDestination(pathPoints))
                {
                    Vector3 pathPoint = pathPoints[0];
                    pathPoints.RemoveAt(0);
                    _navMeshAgent.SetDestination(pathPoint);
                }
            }
            else
            {
                onPathingStopped.Invoke();
                PathingAction = MoveStraight;
            }
        }

        private void MoveStraight(ref PooledList<Vector3> pathPoints)
        {
            if (pathPoints.Count <= 0)
            {
                transform.position += _forwardDirection * Time.deltaTime * _shipSpeed;
            }
            else
            {
                PathingAction = MoveOnPath;
            }
        }

        private bool ShouldSetDestination(List<Vector3> pathPoints)
        {
            if (pathPoints.Count == 0)
                return false;

            if (_navMeshAgent.hasPath == false || _navMeshAgent.remainingDistance < distanceThreshold)
                return true;

            return false;
        }
    }
}