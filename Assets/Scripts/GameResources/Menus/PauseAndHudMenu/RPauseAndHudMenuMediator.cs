using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Menus.PauseAndHudMenu
{
    public class RPauseAndHudMenuMediator : MenuMediator<RPauseAndHudMenuMediator, RPauseAndHudMenuStateMachine, RPauseAndHudMenuView>
    {
        public override void SubscribeToEvents()
        {
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }

            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnEnterMainMenu);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToMainMenu>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnExit, _disposables);
        }

        public void OnEnterPlay(REvent evt)
        {
            View.gameObject.SetActive(true);
            View.settingsButton.gameObject.SetActive(true);
            View.settingsButton.onClick.AddListener(OnSettingsToggled);
            OnEnterMenu();
        }

        public void OnEnterMainMenu(REvent evt)
        {
            View.gameObject.SetActive(true);
            View.settingsButton.gameObject.SetActive(false);
            OnEnterMenu();
        }
        
        public override void OnEnterMenu()
        {
            View.levelText.gameObject.SetActive(true);
            View.scoreText.gameObject.SetActive(true);
            View.pause_MainMenuButton.onClick.AddListener(OnQuitToMainMenu);
        }

        public override void OnExitMenu()
        {
            View.RemoveAllListeners();
        }

        private void OnSettingsToggled()
        {
            if (View.pauseMenu.activeInHierarchy)
            {
                View.pauseMenu.SetActive(false);
                REvent_GameManagerPauseToPlay.Dispatch();
            }
        }

        private void OnQuitToMainMenu()
        {
            REvent_GameManagerPlayToMainMenu.Dispatch();
        }
    }
}