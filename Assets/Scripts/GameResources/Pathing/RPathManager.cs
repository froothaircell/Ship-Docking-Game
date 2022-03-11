using System;
using System.Linq;
using CoreResources.Pool;
using GameResources.Ship;
using UnityEngine;

namespace GameResources.Pathing
{
    public class RPathManager : MonoBehaviour, IShipComponent
    {
        public Action<Vector3> OnNewDestinationSet;
        public float _rendererUpdateRadius = 1f; // determines if the renderer is close enough to the ship to update
        public float _minDistanceToChangeDestination = 0.4f; // must be greater than or equal to line resolution
        public float _lineResolution = 1f; // less increases resolution

        private int _indexForSteering = 0;
        private float _lineRendererYOffset = 0.5f;
        private bool _firstPoint = false;
        private LineRenderer _lineRenderer;
        private PooledList<Vector3> _points;
        private RPathSteering _steeringComponent;
        
        public void OnInit()
        {
            _indexForSteering = 0;
            _firstPoint = false;
            _points = AppHandler.AppPool.Get<PooledList<Vector3>>();
            if (_lineRenderer == null)
                _lineRenderer = GetComponentInChildren<LineRenderer>();
            if (_steeringComponent == null)
                _steeringComponent = GetComponent<RPathSteering>();
        }

        public void OnReset()
        {
            _points.ReturnToPool();
        }

        public void OnUpdate()
        {
            if (_points.Count == 0)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            // remove 0th point only when ship gets to it
            if (Vector3.Distance(_points[0], transform.position) <= _rendererUpdateRadius)
            {
                _points.RemoveAt(0);
                UpdateRenderer();
                if(_indexForSteering > 0)
                    _indexForSteering--;
            }
            
            if (ShouldSetDestination())
            {
                // Get the point that the ship needs via another variable
                OnNewDestinationSet.Invoke(_points[_indexForSteering]);
                _indexForSteering = _indexForSteering == (_points.Count - 1) ? _indexForSteering : _indexForSteering + 1;
            }
        }

        public void BeginPathing()
        {
            ClearPath();
            _firstPoint = true;
        }

        private void ClearPath()
        {
            _points.Clear();
            _lineRenderer.positionCount = 0;
        }
        
        public void DrawPath(RaycastHit hitInfo)
        {
            if (DistanceToLastPoint(hitInfo.point) > _lineResolution)
            {
                Vector3 point = new Vector3(hitInfo.point.x, _lineRendererYOffset, hitInfo.point.z);
                _points.Add(point);
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
                // OnNewPointAdded(point);
            }
        }
        
        private void UpdateRenderer()
        {
            if (_points.Count > 0)
            {
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
            }
        }
        
        private float DistanceToLastPoint(Vector3 newPoint)
        {
            if (!_points.Any())
            {
                return Mathf.Infinity;
            }

            return Vector3.Distance(_points.Last(), newPoint);
        }
        
        private bool ShouldSetDestination()
        {
            if (_points.Count == 0)
                return false;
            if (_firstPoint)
            {
                _firstPoint = false;
                return true;
            }
            var isInRange = Vector3.Distance(transform.position, _points[_indexForSteering]) < _minDistanceToChangeDestination;
            return isInRange;
        }
    }
}