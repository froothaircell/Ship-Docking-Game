using System;
using System.Collections.Generic;
using System.Linq;
using CoreResources.Pool;
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.Pathing
{
    public class RPathingManager : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private PooledList<Vector3> _points;
        public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate {  };
        public float yOffset = 0.5f;

        protected void Awake()
        {
            _points = AppHandler.AppPool.Get<PooledList<Vector3>>();
            _lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        public void ClearPath()
        {
            _points.Clear();
        }

        public void DrawPath(RaycastHit hitInfo)
        {
            if (DistanceToLastPoint(hitInfo.point) > 1f)
            {
                _points.Add(hitInfo.point + new Vector3(0, yOffset));
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
            }
        }

        public void OnButtonReleased()
        {
            OnNewPathCreated(_points);
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