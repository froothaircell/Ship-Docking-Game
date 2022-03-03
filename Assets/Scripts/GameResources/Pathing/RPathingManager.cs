using System;
using System.Linq;
using CoreResources.Pool;
using UnityEngine;

namespace GameResources.Pathing
{
    public class RPathingManager : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private PooledList<Vector3> _points;
        public Action<Vector3> OnNewPointAdded = delegate {  };
        public float yOffset = 0.5f;

        private RPathMover _pathMover;

        public void InitPathing(float speed)
        {
            _points = AppHandler.AppPool.Get<PooledList<Vector3>>();
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _pathMover = GetComponent<RPathMover>();
            _pathMover.onPathingStopped += CleanRenderer;
            _pathMover.InitPathMover(speed);
            OnNewPointAdded += _pathMover.SetPoints;
        }

        private void OnDisable()
        {
            ClearPath();
            CleanRenderer();
            GetComponent<RPathMover>().onPathingStopped -= CleanRenderer;
        }

        public void ClearPath()
        {
            _points.Clear();
            _pathMover.ResetPathMover();
        }

        public void CleanRenderer()
        {
            _lineRenderer.positionCount = 0;
        }

        public void DrawPath(RaycastHit hitInfo)
        {
            if (DistanceToLastPoint(hitInfo.point) > 1f)
            {
                Vector3 point = hitInfo.point + new Vector3(0, yOffset);
                _points.Add(point);
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
                OnNewPointAdded(point);
            }
        }

        public void OnButtonReleased()
        {
        }

        private float DistanceToLastPoint(Vector3 newPoint)
        {
            if (!_points.Any())
            {
                return Mathf.Infinity;
            }

            return Vector3.Distance(_points.Last(), newPoint);
        }
    }
}