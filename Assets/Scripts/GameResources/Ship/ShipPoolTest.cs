using System;
using System.Collections.Generic;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Ship
{
    public class ShipPoolTest : MonoBehaviour
    {
        private List<IDisposable> _disposables;

        private void Start()
        {
            if (_disposables == null)
            {
                _disposables = new List<IDisposable>();
            }
            
            AppHandler.EventHandler.Subscribe<REvent_ShipDocked>(TestCallback, _disposables);
        }

        private void TestCallback(REvent_ShipDocked evt)
        {
            Debug.Log($"Event called at position {evt.Transform.position}");
        }
    }
}