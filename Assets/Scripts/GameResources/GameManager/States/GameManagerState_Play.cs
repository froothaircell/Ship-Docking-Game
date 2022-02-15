using System;
using CoreResources;
using CoreResources.Events;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;

namespace GameResources.GameManager.States
{
    public class GameManagerState_Play : GameManagerState
    {
        private PooledList<IDisposable> _disposables;

        public override void OnEnter()
        {
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();

            AppHandler.EventHandler.Subscribe<REvent_GameManagerPause>(OnPause, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWin>(OnWin, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerLoss>(OnLoss, _disposables);
        }

        public override void OnExit()
        {
            _disposables.ClearDisposables();
        }

        protected override void OnPause(REvent_GameManagerPause evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Pause>();
        }

        protected override void OnWin(REvent_GameManagerWin evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Win>();
        }

        protected override void OnLoss(REvent_GameManagerLoss evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Loss>();
        }
    }
}