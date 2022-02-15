using CoreResources.Events;
using CoreResources.StateMachine;

namespace GameResources.GameManager
{
    // Just to enable the use of switch statements
    public interface IGameManagerState
    {
        
    }
    
    public class GameManagerState : StateHistory<GameManagerStateMachine, GameManagerState, GameManagerStateContext>, IGameManagerState
    {
        public enum GameState
        {
            Undefined = 0,
            MainMenu = 1,
            Play = 2,
            Pause = 3,
            Loss = 4,
            Win = 5
        }

        protected virtual void OnPlay(REvent_GameManagerPlay evt)
        {
            
        }

        protected virtual void OnPause(REvent_GameManagerPause evt)
        {
            
        }

        protected virtual void OnMainMenu(REvent_GameManagerMainMenu evt)
        {
            
        }

        protected virtual void OnWin(REvent_GameManagerWin evt)
        {
            
        }

        protected virtual void OnLoss(REvent_GameManagerLoss evt)
        {
            
        }
    }
}