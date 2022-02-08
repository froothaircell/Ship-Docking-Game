using System;
using System.Collections.Generic;
using CoreResources.Utils;


namespace CoreResources.Handlers.EventHandler
{
    public class REventHandler : GenericSingleton<REventHandler>
    {
        private Dictionary<Type, Delegate> _listeners = new Dictionary<Type, Delegate>();

        // Assigns the dispose function to a collection of IDisposables
        public void Subscribe<T>(Action<T> callback, ICollection<IDisposable> disposableContainer) where T : REvent
        {
            if (disposableContainer == null) throw new ArgumentNullException(nameof(disposableContainer));
            
            disposableContainer.Add(Subscribe(callback));
        }
        
        // Requires you to call the dispose function yourself
        public IDisposable Subscribe<T>(Action<T> callback) where T : REvent
        {
            var type = typeof(T);
            Delegate observer;
            
            if (_listeners.TryGetValue(type, out observer))
            {
                var action = observer as Action<T>;
                action += callback;
                _listeners[type] = action;
            }
            else
            {
                _listeners.Add(type, callback);
            }

            return Disposables.CreateWithState(callback, this.RemoveListener);
        }

        private void RemoveListener<T>(Action<T> callback)
        {
            var type = typeof(T);
            Delegate observer;

            if (_listeners != null && _listeners.TryGetValue(type, out observer))
            {
                var action = observer as Action<T>;
                action -= callback;

                if (action != null)
                {
                    _listeners[type] = action;
                }
                else
                {
                    _listeners.Remove(type);
                }
            }
        }

        public void Dispatch(REvent rEvent, bool returnToPool = true)
        {
            var type = rEvent.GetType();

            if (_listeners.TryGetValue(type, out Delegate observer))
            {
                observer.DynamicInvoke(rEvent);
            }

            if (returnToPool)
            {
                rEvent.ReturnToPool();
            }
        }
    }
}
