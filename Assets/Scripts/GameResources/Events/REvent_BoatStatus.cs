using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace GameResources.Events
{
    public class REvent_BoatsLoaded : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_BoatsLoaded>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_BoatDocked : REvent
    {
        public Vector3 Position;
        
        public static void Dispatch(Vector3 lastPosition)
        {
            var evt = Get<REvent_BoatDocked>();
            evt.Position = lastPosition;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_BoatDestroyed : REvent
    {
        public Vector3 Position;

        public static void Dispatch(Vector3 lastPosition)
        {
            var evt = Get<REvent_BoatDestroyed>();
            evt.Position = lastPosition;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}