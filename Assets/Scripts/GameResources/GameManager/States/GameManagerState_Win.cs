using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;

namespace GameResources.GameManager.States
{
    public class GameManagerState_Win : RGameManagerState
    {
        public override void OnEnter()
        {
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();

            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnMainMenu, _disposables);
        }

        public override void OnExit()
        {
            _disposables.ClearDisposables();
        }
        
        protected override void OnPlay(REvent evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Play>();
        }

        protected override void OnMainMenu(REvent evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_MainMenu>();
        }
    }
}