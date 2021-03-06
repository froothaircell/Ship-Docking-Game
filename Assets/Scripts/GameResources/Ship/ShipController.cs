using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;
using CoreResources.Utils.Disposables;
using GameResources.Events;
using GameResources.ShipDockZone;
using UnityEngine;

namespace GameResources.Ship
{
    [System.Serializable]
    public class ShipData
    {
        public ShipTypes ShipType;
        public ShipColors ShipColor;
        public float ShipSpeed;
    }
    
    public class ShipController : MonoBehaviour
    {
        public ShipData shipData;
        private const float VisibilityThreshold = 5f;
        private bool _firstEntry = false;
        private bool _isPaused = false;
        private Camera _mainCam;
        private PooledList<IDisposable> _disposables;
        private PooledList<IShipComponent> _shipComponents;
        private Action OnOutOfCameraView;

        private void OnEnable()
        {
            InitShipController();
            foreach (var shipComponent in _shipComponents)
            {
                shipComponent.OnInit();
            }
        }

        private void OnDisable()
        {
            foreach (var shipComponent in _shipComponents)
            {
                shipComponent.OnReset();
            }
            CleanShipController();
        }

        private void Update()
        {
            if (_isPaused)
                return;
            
            CheckScreenPosition();
            foreach (var shipComponent in _shipComponents)
            {
                shipComponent.OnUpdate();
            }
        }

        private void InitShipController()
        {
            // Set Ship Color
            switch (shipData.ShipColor)
            {
                case ShipColors.Red:
                    GetComponentInChildren<Renderer>().material.SetColor("Glow_Color", ShipAndMarkerColors.RedM);
                    GetComponentInChildren<LineRenderer>().material.SetColor("_Color", ShipAndMarkerColors.RedM);
                    break;
                case ShipColors.Green:
                    GetComponentInChildren<Renderer>().material.SetColor("Glow_Color", ShipAndMarkerColors.GreenM);
                    GetComponentInChildren<LineRenderer>().material.SetColor("_Color", ShipAndMarkerColors.GreenM);
                    break;
                case ShipColors.Blue:
                    GetComponentInChildren<Renderer>().material.SetColor("Glow_Color", ShipAndMarkerColors.BlueM);
                    GetComponentInChildren<LineRenderer>().material.SetColor("_Color", ShipAndMarkerColors.BlueM);
                    break;
                default:
                    GetComponentInChildren<Renderer>().material.SetColor("Glow_Color", Color.white);
                    GetComponentInChildren<LineRenderer>().material.SetColor("_Color", Color.white);
                    break;
            }

            // init visibility based event
            _mainCam = Camera.main;
            _firstEntry = false;
            _isPaused = false;
            OnOutOfCameraView += OnInvisible;
            
            // init pooled lists
            _shipComponents = AppHandler.AppPool.Get<PooledList<IShipComponent>>();
            _shipComponents = GetComponents<IShipComponent>().ToPooledList();
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            
            // init event based callbacks
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnPause, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToPlay>(OnResume, _disposables);
        }

        private void CleanShipController()
        {
            _firstEntry = false;
            OnOutOfCameraView -= OnInvisible;

            _shipComponents?.ReturnToPool();

            _disposables?.ClearDisposables();
            _disposables?.ReturnToPool();
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
                    ((shipScreenPosition.x + VisibilityThreshold) < 0 ||
                     (shipScreenPosition.x - VisibilityThreshold) > Screen.width ||
                     (shipScreenPosition.y + VisibilityThreshold) < 0 ||
                     (shipScreenPosition.y - VisibilityThreshold) > Screen.height))
                {
                    OnOutOfCameraView.Invoke();
                }
            }
        }

        private void OnPause(REvent evt)
        {
            _isPaused = true;
        }

        private void OnResume(REvent evt)
        {
            _isPaused = false;
        }
        
        private void OnInvisible()
        {
            REvent_ShipDestroyed.Dispatch(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            {
                ShipColors assignedDockColor = other.transform.parent.GetComponent<DockZoneController>().assignedShipColor;
                if (assignedDockColor == shipData.ShipColor)
                {
                    REvent_ShipDocked.Dispatch(transform);
                }
                else
                {
                    REvent_ShipDestroyed.Dispatch(transform);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                     other.gameObject.layer == LayerMask.NameToLayer("Ship"))
            {
                REvent_ShipDestroyed.Dispatch(transform);
            }
        }
    }
}