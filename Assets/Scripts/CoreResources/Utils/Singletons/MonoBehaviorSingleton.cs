using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace CoreResources.Utils.Singletons
{
    public abstract class MonobehaviorSingleton<T> : MonoBehaviour where T : MonobehaviorSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }
        
        protected virtual void InitSingleton()
        {
            if (Instance != null && GetInstanceID() != Instance.GetInstanceID())
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void Awake()
        {
            InitSingleton();
        }

        protected virtual void OnReset(REvent evt)
        {
            
        }
    }
}