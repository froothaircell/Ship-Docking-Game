using System;
using CoreResources;
using CoreResources.Events;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;

namespace GameResources.GameManager.States
{
    public class GameManagerState_Pause : GameManagerState
    {
        private PooledList<IDisposable> _disposables;

        public override void OnEnter()
        {
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();

            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlay>(OnPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenu>(OnMainMenu, _disposables);
        }

        public override void OnExit()
        {
            _disposables.ClearDisposables();
        }
        
        protected override void OnPlay(REvent_GameManagerPlay evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Play>();
        }

        protected override void OnMainMenu(REvent_GameManagerMainMenu evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_MainMenu>();
        }
    }
}