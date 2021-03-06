using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;

namespace GameResources.GameManager.States
{
    public class GameManagerState_Play : RGameManagerState
    {
        public override void OnEnter()
        {
            _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();

            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnPause, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnWin, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnLoss, _disposables);
        }

        public override void OnExit()
        {
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
                _disposables.ReturnToPool();
            }
        }

        protected override void OnPause(REvent evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Pause>();
        }

        protected override void OnWin(REvent evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Win>();
        }

        protected override void OnLoss(REvent evt)
        {
            AppHandler.GMMediator.FSM.GoToState<GameManagerState_Loss>();
        }
    }
}