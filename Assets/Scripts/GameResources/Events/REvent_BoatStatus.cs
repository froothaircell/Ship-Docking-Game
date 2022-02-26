using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace GameResources.Events
{
    public class REvent_BoatDocked : REvent
    {
        public Vector3 position;
        
        public static void Dispatch(Vector3 lastPosition)
        {
            var evt = Get<REvent_BoatDocked>();
            evt.position = lastPosition;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_BoatDestroyed : REvent
    {
        public Vector3 position;

        public static void Dispatch(Vector3 lastPosition)
        {
            var evt = Get<REvent_BoatDestroyed>();
            evt.position = lastPosition;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}