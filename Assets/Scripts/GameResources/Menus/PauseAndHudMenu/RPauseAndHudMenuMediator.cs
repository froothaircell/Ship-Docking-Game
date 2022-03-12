using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using DG.Tweening;
using GameResources.Events;

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
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToMainMenu>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnEnterMainMenu, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnExit, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnExit, _disposables);

            // Stats based events
            AppHandler.EventHandler.Subscribe<REvent_GameStart>(OnDisplayLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnDisplayLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToMainMenu>(OnDisplayLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_DisplayDockedProgress>(OnDisplayDockedShipProgress, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_DisplayDestroyedProgress>(OnDisplayDestroyedShipProgress,
                _disposables);
            
            View.settingsButton.onClick.AddListener(OnSettingsToggled);
            View.pause_MainMenuButton.onClick.AddListener(OnQuitToMainMenu);
            View.pause_ResetSavesButton.onClick.AddListener(OnResetSaves);
        }

        public void OnEnterPlay(REvent evt)
        {
            View.scoreText.gameObject.SetActive(false);
            View.dockedShipsProgress.SetActive(true);
            View.destroyedShipsProgress.SetActive(true);
            View.settingsButton.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            OnEnterMenu();
        }

        public void OnEnterMainMenu(REvent evt)
        {
            View.scoreText.gameObject.SetActive(true);
            View.dockedShipsProgress.SetActive(false);
            View.destroyedShipsProgress.SetActive(false);
            View.pauseMenu.SetActive(false);
            View.settingsButton.gameObject.SetActive(false);
            OnEnterMenu();
            DisplayScore(AppHandler.PlayerStats.Score);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        public override void OnEnterMenu()
        {
            View.levelText.gameObject.SetActive(true);
            View.dockedShipsProgressBar.value = 0f;
            View.destroyedShipsProgressBar.value = 0f;

        }

        public override void OnExitMenu()
        {
            
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
            AppHandler.LevelHandler.LoadMainMenu();
        }

        private void OnResetSaves()
        {
            AppHandler.SaveManager.ResetPlayerPrefs();
            AppHandler.PlayerStats.ResetStats();
            AppHandler.LevelHandler.LoadMainMenu();
        }

        private void OnDisplayLevel(REvent evt)
        {
            DisplayLevel(AppHandler.PlayerStats.Level);
        }

        private void DisplayLevel(int level)
        {
            View.levelText.text = "Level " + level;
        }

        private void OnDisplayDockedShipProgress(REvent_DisplayDockedProgress evt)
        {
            DisplayDockedShipProgress(evt.DockedProgress);
        }

        private void OnDisplayDestroyedShipProgress(REvent_DisplayDestroyedProgress evt)
        {
            DisplayDestroyedShipProgress(evt.DestroyedProgress);
        }

        private void DisplayScore(int score)
        {
            View.scoreText.text = "Score" + score;
        }

        private void DisplayDockedShipProgress(float progress)
        {
            DOTween.To(() => View.dockedShipsProgressBar.value, 
                x => View.dockedShipsProgressBar.value = x, progress, 2f);
        }

        private void DisplayDestroyedShipProgress(float progress)
        {
            DOTween.To(() => View.destroyedShipsProgressBar.value, 
                x => View.destroyedShipsProgressBar.value = x, progress, 2f);
        }
    }
}