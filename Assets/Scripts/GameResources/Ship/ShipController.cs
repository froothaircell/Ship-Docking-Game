using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;
using GameResources.Pathing;
using UnityEngine;

namespace GameResources.Ship
{
    [System.Serializable]
    public class ShipData
    {
        public ShipTypes ShipType;
        public float ShipSpeed;
    }

    public class ShipController : MonoBehaviour
    {
        public ShipData _shipData;
        private static float visibilityThreshold = 5f;
        private RPathingManager _pathingManager;
        private PooledList<IDisposable> _disposables;

        private void Awake()
        {
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }
            
            _pathingManager = GetComponent<RPathingManager>();
        }

        private void OnEnable()
        {
            _pathingManager.InitPathing(_shipData.ShipSpeed, visibilityThreshold);
            _pathingManager.OnOutOfCameraView += OnInvisible;
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnPause, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToPlay>(OnResume, _disposables);
        }

        private void OnDisable()
        {
            _pathingManager.OnOutOfCameraView -= OnInvisible;
            _pathingManager.DisablePathingManager();
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
                _disposables.ReturnToPool();
            }
        }

        private void OnPause(REvent evt)
        {
            _pathingManager.PausePath();
        }

        private void OnResume(REvent evt)
        {
            _pathingManager.ResumePathing();
        }

        private void OnInvisible()
        {
            REvent_BoatDestroyed.Dispatch(transform.position);
            AppHandler.ShipPoolHandler.AddToPool(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            {
                REvent_BoatDocked.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.AddToPool(gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                     other.gameObject.layer == LayerMask.NameToLayer("Boat"))
            {
                REvent_BoatDestroyed.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.AddToPool(gameObject);
            }
        }
    }
}