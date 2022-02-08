using System.Collections.Generic;
using System;
using CoreResources.Pool;
using UnityEngine;

namespace CoreResources.Handlers.EventHandler
{
    class Event1 : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<Event1>();

            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    class Event2 : REvent
    {
        public int Something;

        public static void Dispatch(int val)
        {
            var evt = Get<Event2>();
            evt.Something = val;
            Debug.Log($"The value stored is {val}");
            
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    class Event3 : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<Event3>();

            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class EventHandlerTest : MonoBehaviour
    {
        private PooledList<IDisposable> _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>(); // Get from pool

        private void Start()
        {
            AppHandler.EventHandler.Subscribe<Event1>(Event1Callback, _disposables);
            AppHandler.EventHandler.Subscribe<Event2>(Event2Callback, _disposables);
            AppHandler.EventHandler.Subscribe<Event3>(Event3Callback, _disposables);
            AppHandler.EventHandler.Subscribe<Event3>(Event3AltCallback, _disposables);
            
            Event2.Dispatch(7);
            Event1.Dispatch();
            Event3.Dispatch();
            Debug.Log("We got here");
        }
        
        void Event1Callback(Event1 evt)
        {
            Debug.Log("Event1 called");
        }
        
        void Event2Callback(Event2 evt)
        {
            Debug.Log("Event2 called");
        }
        
        void Event3Callback(Event3 evt)
        {
            Debug.Log("Event3 called");
        }

        void Event3AltCallback(Event3 evt)
        {
            Debug.Log("An extra callback attached to Event3");
        }
    }
}