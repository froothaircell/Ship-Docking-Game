using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;
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

            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToMainMenu>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnExit, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnExit, _disposables);
        }

        public void OnEnterPlay(REvent evt)
        {
            View.settingsButton.gameObject.SetActive(true);
            View.settingsButton.onClick.AddListener(OnSettingsToggled);
            transform.GetChild(0).gameObject.SetActive(true);
            OnEnterMenu();
        }

        public void OnEnterMainMenu(REvent evt)
        {
            View.pauseMenu.SetActive(false);
            View.RemoveAllListeners();
            View.settingsButton.gameObject.SetActive(false);
            View.levelText.gameObject.SetActive(true);
            View.scoreText.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        public override void OnEnterMenu()
        {
            View.levelText.gameObject.SetActive(true);
            View.scoreText.gameObject.SetActive(true);
            View.pause_MainMenuButton.onClick.AddListener(OnQuitToMainMenu);
            View.pause_ResetSavesButton.onClick.AddListener(OnResetSaves);
        }

        public override void OnExitMenu()
        {
            View.RemoveAllListeners();
            // _disposables.ClearDisposables();
        }

        private void OnSettingsToggled()
        {
            if (View.pauseMenu.activeInHierarchy)
            {
                View.pauseMenu.SetActive(false);
                REvent_GameManagerPauseToPlay.Dispatch();
            }
            else
            {
                View.pauseMenu.SetActive(true);
                REvent_GameManagerPlayToPause.Dispatch();
            }
        }

        private void OnQuitToMainMenu()
        {
            LevelManager.LoadMainMenu();
        }

        private void OnResetSaves()
        {
            AppHandler.SaveManager.ResetPlayerPrefs();
            AppHandler.PlayerStats.ResetStats();
            LevelManager.LoadMainMenu();
        }
    }
}