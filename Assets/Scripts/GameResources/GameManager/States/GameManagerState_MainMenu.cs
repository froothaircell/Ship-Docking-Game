using System;
using CoreResources;
using CoreResources.Events;

namespace GameResources.GameManager.States
{
    public class GameManagerState_MainMenu : GameManagerState
    {
        private IDisposable _playDisposable;
        
        public override void OnEnter()
        {
            _playDisposable = AppHandler.EventHandler.Subscribe<REvent_GameManagerPlay>(OnPlay);
        }

        public override void OnExit()
        {
            _playDisposable.Dispose();
        }

        protected override void OnPlay(REvent_GameManagerPlay evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Play>();
        }
    }
}