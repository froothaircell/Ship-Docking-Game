using System;
using CoreResources.Handlers.EventHandler;
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

        protected virtual void OnPlay(REvent evt)
        {
            
        }

        protected virtual void OnPause(REvent evt)
        {
            
        }

        protected virtual void OnMainMenu(REvent evt)
        {
            
        }

        protected virtual void OnWin(REvent evt)
        {
            
        }

        protected virtual void OnLoss(REvent evt)
        {
            
        }
    }
}