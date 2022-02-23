using UnityEngine;

namespace CoreResources.Utils.Singletons
{
    public abstract class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
    {
        private static T _instance;
        protected bool FirstInitComplete = false;

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
            if (Instance != null && FirstInitComplete)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
                FirstInitComplete = true; // Makes sure that Instance can be called before awake
            }
        }

        protected virtual void Awake()
        {
            InitSingleton();
        }
    }
}