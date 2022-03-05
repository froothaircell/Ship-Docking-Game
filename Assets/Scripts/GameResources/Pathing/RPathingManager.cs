using System;
using System.Collections;
using System.Linq;
using CoreResources.Pool;
using CoreResources.Utils.Jobs;
using UnityEngine;

namespace GameResources.Pathing
{
    public class RPathingManager : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private PooledList<Vector3> _points;
        public float yOffset = 0.5f;
        public Action<Vector3> OnNewPointAdded = delegate {  };
        private UpdateJob PathingCoroutine;

        private RPathMover _pathMover;

        public void InitPathing(float speed)
        {
            _points = AppHandler.AppPool.Get<PooledList<Vector3>>();
            if (_lineRenderer == null)
                _lineRenderer = GetComponentInChildren<LineRenderer>();
            if (_pathMover == null)
            {
                _pathMover = GetComponent<RPathMover>();
            }
            _pathMover.InitPathMover(speed);
            _pathMover.onPathingStopped += CleanRenderer;
            BeginPathing();
        }

        public void DisablePathingManager()
        {
            FinishPathing();
            ClearPoints();
            _pathMover.ResetPathMover();
            GetComponent<RPathMover>().onPathingStopped -= CleanRenderer;
            CleanRenderer();
        }

        private void BeginPathing()
        {
            if (PathingCoroutine == null)
            {
                PathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(UpdatePath());
            }
            else
            {
                JobManager.SafeStopUpdate(ref PathingCoroutine);
                PathingCoroutine = AppHandler.JobHandler.ExecuteCoroutine(UpdatePath());
            }
        }

        private void FinishPathing()
        {
            if (PathingCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref PathingCoroutine);
            }
        }

        public void ClearPoints()
        {
            _points.Clear();
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
                // OnNewPointAdded(point);
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
        
        private IEnumerator UpdatePath()
        {
            while (true)
            {
                _pathMover.MoveAgent(ref _points);
                UpdateRenderer();
                yield return 0;
            }
        }

        public void UpdateRenderer()
        {
            if (_points.Count > 0)
            {
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
            }
        }

        public void OnButtonReleased()
        {
        }
    }
}