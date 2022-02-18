using CoreResources.Utils;
using UnityEngine;

namespace CoreResources.Mediators
{
    public class Mediator<T> : GenericSingleton<T> where T : Mediator<T>
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }
        
        public static T Create()
        {
            GameObject mediatorGO = new GameObject();
            T mediator = mediatorGO.AddComponent<T>();
            mediatorGO.name = mediator.Name;
            
            // mediator.Initialize();
            
            return mediator;
        }

        public virtual void Initialize()
        {
            Debug.Log($"{Name} Initialized!");
        }
    }
}