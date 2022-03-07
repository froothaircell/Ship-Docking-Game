using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace GameResources.Events
{
    public class REvent_ShipsLoaded : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_ShipsLoaded>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_ShipDocked : REvent
    {
        public Transform Transform;
        
        public static void Dispatch(Transform shipTransform)
        {
            var evt = Get<REvent_ShipDocked>();
            evt.Transform = shipTransform;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_ShipDestroyed : REvent
    {
        public Transform Transform;

        public static void Dispatch(Transform shipTransform)
        {
            var evt = Get<REvent_ShipDestroyed>();
            evt.Transform = shipTransform;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}