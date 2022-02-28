using CoreResources.Utils.Singletons;
using GameResources.Pathing;
using UnityEngine;

namespace GameResources.InputManagement
{
    public class RInputManager : GenericSingleton<RInputManager>
    {
        private LayerMask _boatLayerMask;
        private RPathingManager _selectedPathingManager;
        private RaycastHit _raycastHit;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _selectedPathingManager = null;
        }

        private void Start()
        {
            _boatLayerMask = LayerMask.GetMask("Boat");
        }

        private void Update()
        {
            HandleInput();
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
                    _selectedPathingManager.ClearPath();
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
            
#endif
        }
    }
}