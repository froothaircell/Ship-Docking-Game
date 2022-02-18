using System;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;

namespace GameResources.GameManager.States
{
    public class GameManagerState_MainMenu : RGameManagerState
    {
        public override void OnEnter()
        {
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();

            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnPlay, _disposables);
        }

        public override void OnExit()
        {
            _disposables.ClearDisposables();
        }

        protected override void OnPlay(REvent_GameManagerMainMenuToPlay evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Play>();
        }
    }
}