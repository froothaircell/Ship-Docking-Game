using CoreResources.Handlers.EventHandler;

namespace GameResources.Events
{
    public class REvent_DisplayScore : REvent
    {
        public int Score;
        
        public static void Dispatch(int score)
        {
            var evt = Get<REvent_DisplayScore>();
            evt.Score = score;
            AppHandler.EventHandler.Dispatch(evt);
        }
    }
}