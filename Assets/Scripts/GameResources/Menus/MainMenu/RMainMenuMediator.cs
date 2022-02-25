using System;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;

namespace GameResources.Menus.MainMenu
{
    public class RMainMenuMediator : MenuMediator<RMainMenuMediator, RMainMenuStateMachine, RMainMenuView>
    {
        public override void SubscribeToEvents()
        {
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }


            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnEnter, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnExit, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToMainMenu>(OnEnter, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnEnter, _disposables);
        }

        public override void OnEnterMenu()
        {
            View.StartGameButton.onClick.AddListener(OnStartGame);
            View.QuitGameButton.onClick.AddListener(OnQuitGame);
        }

        public override void OnExitMenu()
        {
            View.RemoveAllListeners();
            // _disposables.ClearDisposables();
        }

        private void OnStartGame()
        {
            REvent_GameManagerMainMenuToPlay.Dispatch();
        }

        private void OnQuitGame()
        {
            REvent_GameQuit.Dispatch();
        }
    }
}