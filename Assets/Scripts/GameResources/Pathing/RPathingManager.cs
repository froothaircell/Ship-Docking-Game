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
        public float yOffset = 0.5f;
        public Action OnOutOfCameraView = delegate { };
        private LineRenderer _lineRenderer;
        private PooledList<Vector3> _points;
        private UpdateJob PathingCoroutine;
        private float _visibilityThreshold = 0f;
        private bool _firstEntry = false;
        private Camera _mainCam;

        private RPathMover _pathMover;

        public void InitPathing(float speed, float visibilityThreshold)
        {
            _points = AppHandler.AppPool.Get<PooledList<Vector3>>();
            _mainCam = Camera.main;
            if (_lineRenderer == null)
                _lineRenderer = GetComponentInChildren<LineRenderer>();
            if (_pathMover == null)
                _pathMover = GetComponent<RPathMover>();
            
            _pathMover.InitPathMover(speed);
            _visibilityThreshold = visibilityThreshold;
            _firstEntry = false;
            _pathMover.onPathingStopped += CleanRenderer;
            BeginPathing();
        }

        public void DisablePathingManager()
        {
            _firstEntry = false;
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

        public void PausePath()
        {
            FinishPathing();
        }

        public void ResumePathing()
        {
            BeginPathing();
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
                CheckScreenPosition();
                yield return 0;
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

        private void CheckScreenPosition()
        {
            if (_mainCam != null)
            {
                Vector3 shipScreenPosition = _mainCam.WorldToScreenPoint(transform.position);
                if (!_firstEntry && 
                    (shipScreenPosition.x > 0 && 
                     shipScreenPosition.x < Screen.width && 
                     shipScreenPosition.y > 0 && 
                     shipScreenPosition.y < Screen.height))
                {
                    _firstEntry = true;
                    return;
                }
                if (_firstEntry &&
                    ((shipScreenPosition.x + _visibilityThreshold) < 0 ||
                     (shipScreenPosition.x - _visibilityThreshold) > Screen.width ||
                     (shipScreenPosition.y + _visibilityThreshold) < 0 ||
                     (shipScreenPosition.y - _visibilityThreshold) > Screen.height))
                {
                    OnOutOfCameraView.Invoke();
                }
            }
        }

        public void OnButtonReleased()
        {
        }
    }
}