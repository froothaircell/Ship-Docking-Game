using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.InputManagement
{
    public class InputManager : GenericSingleton<InputManager>
    {
        private void HandleInput()
        {
#if UNITY_EDITOR
            if (Input.GetButtonDown("Fire1"))
            {
                
            }
#else
            
#endif
        }
    }
}