using System;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;

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


            // AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnEnter, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnEnter, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnExit, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToMainMenu>(OnEnter, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnEnter, _disposables);

            View.StartGameButton.onClick.AddListener(OnStartGame);
            View.QuitGameButton.onClick.AddListener(OnQuitGame);
        }

        public override void OnEnterMenu()
        {
        }

        public override void OnExitMenu()
        {
        }

        private void OnStartGame()
        {
            LevelManager.LoadCurrentLevel();
            REvent_GameManagerMainMenuToPlay.Dispatch();
        }

        private void OnQuitGame()
        {
            REvent_GameQuit.Dispatch();
        }
    }
}