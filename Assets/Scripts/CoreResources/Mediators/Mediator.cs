using UnityEngine;

namespace CoreResources.Mediators
{
    public class Mediator : MonoBehaviour
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }

    public class Mediator<T> : Mediator where T : Mediator<T>
    {
        static public T Create()
        {
            GameObject mediatorGO = new GameObject();
            T mediator = mediatorGO.AddComponent<T>();
            mediatorGO.name = mediator.Name;
            
            mediator.Initialize();
            
            return mediator;
        }

        protected virtual void Initialize()
        {
            Debug.Log($"{Name} Initialized!");
        }
    }
}