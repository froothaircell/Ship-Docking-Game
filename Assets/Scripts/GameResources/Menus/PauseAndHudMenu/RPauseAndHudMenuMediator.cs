using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;

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

            // Stats based events
            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnDisplayLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnDisplayLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_DisplayScore>(OnDisplayScore, _disposables);
            
            View.settingsButton.onClick.AddListener(OnSettingsToggled);
            View.pause_MainMenuButton.onClick.AddListener(OnQuitToMainMenu);
            View.pause_ResetSavesButton.onClick.AddListener(OnResetSaves);
        }

        public void OnEnterPlay(REvent evt)
        {
            View.settingsButton.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            DisplayScore(0);
            OnEnterMenu();
        }

        public void OnEnterMainMenu(REvent evt)
        {
            View.pauseMenu.SetActive(false);
            View.settingsButton.gameObject.SetActive(false);
            OnEnterMenu();
            DisplayScore(AppHandler.PlayerStats.Score);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        public override void OnEnterMenu()
        {
            View.levelText.gameObject.SetActive(true);
            View.scoreText.gameObject.SetActive(true);
        }

        public override void OnExitMenu()
        {
            // View.RemoveAllListeners();
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

        private void OnDisplayLevel(REvent evt)
        {
            DisplayLevel(AppHandler.PlayerStats.Level);
        }

        private void DisplayLevel(int level)
        {
            View.levelText.text = "Level " + level;
        }

        private void OnDisplayScore(REvent_DisplayScore evt)
        {
            DisplayScore(evt.Score);
        }

        private void DisplayScore(int score)
        {
            View.scoreText.text = "Score " + score;
        }
    }
}