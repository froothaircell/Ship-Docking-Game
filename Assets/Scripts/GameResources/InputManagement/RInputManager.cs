using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.Pathing;
using UnityEngine;

namespace GameResources.InputManagement
{
    public class RInputManager : GenericSingleton<RInputManager>
    {
        private LayerMask _boatLayerMask;
        private RPathingManager _selectedPathingManager;
        private RaycastHit _raycastHit;
        private PooledList<IDisposable> _disposables;
        private bool _disableInput = false;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _selectedPathingManager = null;
            _disableInput = false;
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }

            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnPauseInputs, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnPauseInputs, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnPauseInputs, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnResumeInputs, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToPlay>(OnResumeInputs, _disposables);
        }

        private void Start()
        {
            _boatLayerMask = LayerMask.GetMask("Boat");
        }

        private void Update()
        {
            if (!_disableInput)
            {
                HandleInput();
            }
        }

        private void OnDestroy()
        {
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
                _disposables.ReturnToPool();
            }
        }

        private void OnPauseInputs(REvent evt)
        {
            _disableInput = true;
        }

        private void OnResumeInputs(REvent evt)
        {
            _disableInput = false;
        }

        private void HandleInput()
        {
            Ray ray;
#if UNITY_EDITOR
            if (Input.GetButtonDown("Fire1"))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _raycastHit, Mathf.Infinity, (_boatLayerMask)))
                {
                    _selectedPathingManager = _raycastHit.collider.transform.parent.GetComponent<RPathingManager>();
                    _selectedPathingManager.ClearPoints();
                }
            }

            if (Input.GetButtonUp("Fire1") && _selectedPathingManager != null)
            {
                _selectedPathingManager.OnButtonReleased();
                _selectedPathingManager = null;
            }

            if (Input.GetButton("Fire1") && _selectedPathingManager != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _raycastHit, Mathf.Infinity))
                {
                    _selectedPathingManager.DrawPath(_raycastHit);
                }
            }
#else
            if (Input.touchCount > 0)
            {
                Touch press = Input.GetTouch(0);
                if (press.phase == TouchPhase.Began)
                {
                    ray = Camera.main.ScreenPointToRay(press.position);
                    if (Physics.Raycast(ray, out _raycastHit, Mathf.Infinity, (_boatLayerMask)))
                    {
                        _selectedPathingManager = _raycastHit.collider.transform.parent.GetComponent<RPathingManager>();
                        _selectedPathingManager.ClearPoints();
                    }
                }

                if (press.phase == TouchPhase.Ended)
                {
                    _selectedPathingManager.OnButtonReleased();
                    _selectedPathingManager = null;
                }

                if (press.phase == TouchPhase.Moved && _selectedPathingManager != null)
                {
                    ray = Camera.main.ScreenPointToRay(press.position);
                    if (Physics.Raycast(ray, out _raycastHit, Mathf.Infinity))
                    {
                        _selectedPathingManager.DrawPath(_raycastHit);
                    }
                }
            }
#endif
        }
    }
}