using System;
using System.Collections.Generic;
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.Pathing
{
    public class PathingManager : GenericSingleton<PathingManager>
    {
        private LineRenderer _lineRenderer;
        private List<Vector3> _points = new List<Vector3>();
        private Action<IEnumerable<Vector3>> OnNewPathCreated = delegate {  };

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _lineRenderer = GetComponent<LineRenderer>();
        }
    }
}