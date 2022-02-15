using CoreResources.Handlers.EventHandler;

namespace CoreResources.Events
{
    public class REvent_GameManagerPlay : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPlay>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerPause : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPause>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerMainMenu : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerMainMenu>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerWin : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerWin>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerLoss : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerLoss>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}