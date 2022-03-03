using System;

namespace CoreResources.Utils.Singletons
{
    public abstract class GenericSingleton
    {
        protected abstract void InitSingleton();
        protected abstract void CleanSingleton();
    }
    
    public abstract class InitializableGenericSingleton<T> : GenericSingleton where T : InitializableGenericSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"Accessing {typeof(T).Name} before {typeof(T).Name}.");
                }
                
                return _instance;
            }
        }

        public static bool IsInstantiated => _instance != null;

        public static TCustom SetInstanceType<TCustom>() where TCustom : T, new()
        {
            TCustom newInstance;

            if (_instance == null)
            {
                newInstance = new TCustom();
                _instance = newInstance;
                _instance.InitSingleton();
            }
            else
            {
                newInstance = _instance as TCustom;

                if (newInstance == null)
                {
                    _instance.CleanSingleton();
                    newInstance = new TCustom();
                    _instance = newInstance;
                    _instance.InitSingleton();
                }
            }

            return newInstance;
        }

        protected override void InitSingleton()
        {
            
        }

        protected override void CleanSingleton()
        {
            
        }
    }
}