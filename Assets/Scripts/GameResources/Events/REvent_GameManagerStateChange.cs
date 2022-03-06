using CoreResources.Handlers.EventHandler;

namespace GameResources.Events
{
    public class REvent_GameStart : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameStart>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_LevelStart : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_LevelStart>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerMainMenuToPlay : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerMainMenuToPlay>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerWinOrLossToPlay : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerWinOrLossToPlay>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerPlayToPause : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPlayToPause>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_GameManagerPauseToPlay : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPauseToPlay>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerPauseToMainMenu : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPauseToMainMenu>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }

    public class REvent_GameManagerWinOrLossToMainMenu : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerWinOrLossToMainMenu>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerPlayToWin : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPlayToWin>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameManagerPlayToLoss : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameManagerPlayToLoss>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
    
    public class REvent_GameQuit : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameQuit>();
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}