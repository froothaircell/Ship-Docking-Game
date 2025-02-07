using System;
using CoreResources.Mediators;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;
using UnityEngine;

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
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToMainMenu>(OnEnter, _disposables);
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
            AppHandler.LevelHandler.LoadCurrentLevel();
            REvent_GameManagerMainMenuToPlay.Dispatch();
        }

        private void OnQuitGame()
        {
            REvent_GameQuit.Dispatch();
            Application.Quit();
        }
    }
}