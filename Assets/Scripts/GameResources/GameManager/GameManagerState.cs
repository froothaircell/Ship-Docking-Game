using System;
using CoreResources.Pool;
using CoreResources.StateMachine;
using GameResources.Events;

namespace GameResources.GameManager
{
    // Just to enable the use of switch statements
    public interface IGameManagerState
    {
        
    }
    
    public class RGameManagerState : StateHistory<RGameManagerStateMachine, RGameManagerState, RGameManagerStateContext>, IGameManagerState
    {
        protected PooledList<IDisposable> _disposables;
        
        public enum GameState
        {
            Undefined = 0,
            MainMenu = 1,
            Play = 2,
            Pause = 3,
            Loss = 4,
            Win = 5
        }

        protected virtual void OnPlay(REvent_GameManagerMainMenuToPlay evt)
        {
            
        }

        protected virtual void OnPause(REvent_GameManagerPlayToPause evt)
        {
            
        }

        protected virtual void OnMainMenu(REvent_GameManagerPlayToMainMenu evt)
        {
            
        }

        protected virtual void OnWin(REvent_GameManagerPlayToWin evt)
        {
            
        }

        protected virtual void OnLoss(REvent_GameManagerPlayToLoss evt)
        {
            
        }
    }
}