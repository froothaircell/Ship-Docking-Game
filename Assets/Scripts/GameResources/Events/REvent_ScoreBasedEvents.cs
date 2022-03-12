using CoreResources.Handlers.EventHandler;

namespace GameResources.Events
{
    public class REvent_DisplayDockedProgress : REvent
    {
        public float DockedProgress;
        
        public static void Dispatch(float dockedProgress)
        {
            var evt = Get<REvent_DisplayDockedProgress>();
            evt.DockedProgress = dockedProgress;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_DisplayDestroyedProgress : REvent
    {
        public float DestroyedProgress;

        public static void Dispatch(float destroyedProgress)
        {
            var evt = Get<REvent_DisplayDestroyedProgress>();
            evt.DestroyedProgress = destroyedProgress;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}