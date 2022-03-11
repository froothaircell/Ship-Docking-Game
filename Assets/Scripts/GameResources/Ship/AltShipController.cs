using System;
using CoreResources.Pool;
using CoreResources.Utils;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Ship
{
    // Add Ship data class here later

    public class AltShipController : MonoBehaviour
    {
        public ShipData shipData;
        private const float VisibilityThreshold = 5f;
        private bool _firstEntry = false;
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
            CheckScreenPosition();
            foreach (var shipComponent in _shipComponents)
            {
                shipComponent.OnUpdate();
            }
        }

        private void InitShipController()
        {
            // init visibility based event
            _mainCam = Camera.main;
            _firstEntry = false;
            OnOutOfCameraView += OnInvisible;
            
            // init pooled lists
            _shipComponents = AppHandler.AppPool.Get<PooledList<IShipComponent>>();
            _shipComponents = GetComponents<IShipComponent>().ToPooledList();
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
        }

        private void CleanShipController()
        {
            _firstEntry = false;
            OnOutOfCameraView -= OnInvisible;
            _shipComponents.ReturnToPool();
            _disposables.ReturnToPool();
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
        
        private void OnInvisible()
        {
            REvent_ShipDestroyed.Dispatch(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            {
                REvent_ShipDocked.Dispatch(transform);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                     other.gameObject.layer == LayerMask.NameToLayer("Ship"))
            {
                REvent_ShipDestroyed.Dispatch(transform);
            }
        }
    }
}