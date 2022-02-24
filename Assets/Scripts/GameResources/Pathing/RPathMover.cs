using System.Collections;
using System.Collections.Generic;
using CoreResources.Utils.Jobs;
using UnityEngine;
using UnityEngine.AI;

namespace GameResources.Pathing
{
    public class RPathMover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Queue<Vector3> _pathPoints = new Queue<Vector3>();
        public float distanceThreshold = 0.5f;
        private UpdateJob _pathingCoroutine;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            GetComponent<RPathingManager>().OnNewPathCreated += SetPoints;
        }

        private void SetPoints(IEnumerable<Vector3> points)
        {
            _pathPoints = new Queue<Vector3>(points);
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

        public IEnumerator UpdatePathing()
        {
            while (true)
            {
                if (ShouldSetDestination())
                {
                    Vector3 pathPoint = _pathPoints.Dequeue();
                    _navMeshAgent.SetDestination(pathPoint);
                    yield return 0;
                }
                else
                {
                    // Add logic here to maintain course in the current direction of the ship
                    
                    yield return 0;
                }
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