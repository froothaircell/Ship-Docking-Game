using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;

namespace GameResources.Menus.WinOrLossMenu
{
    public class RWinOrLossMenuMediator : MenuMediator<RWinOrLossMenuMediator, RWinOrLossMenuStateMachine, RWinOrLossMenuView>
    {
        public override void SubscribeToEvents()
        {
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }
            
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnEnterLoss, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnEnterWin, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnExit, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnExit, _disposables);
            
            View.nextLevelButton.onClick.AddListener(OnNextLevel);
            View.mainMenuButton.onClick.AddListener(OnMainMenu);
            View.restartLevelButton.onClick.AddListener(OnRestartLevel);
        }
        
        public void OnEnterWin(REvent evt)
        {
            View.gameObject.SetActive(true);
            View.winOrLossText.text = string.Format($"You Won! \n Total Score : {AppHandler.PlayerStats.Score}");
            OnEnterMenu();
            View.nextLevelButton.gameObject.SetActive(true);
        }

        public void OnEnterLoss(REvent evt)
        {
            View.gameObject.SetActive(true);
            View.winOrLossText.text = string.Format($"You Lost! \n");
            View.nextLevelButton.gameObject.SetActive(false);
            OnEnterMenu();
        }
        
        public override void OnEnterMenu()
        {
            View.mainMenuButton.gameObject.SetActive(true);
            View.restartLevelButton.gameObject.SetActive(true);
            View.winOrLossText.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public override void OnExitMenu()
        {
            // View.RemoveAllListeners();
            // _disposables.ClearDisposables();
        }

        private void OnNextLevel()
        {
            LevelManager.LoadNextLevel();
            REvent_GameManagerWinOrLossToPlay.Dispatch();
        }

        private void OnMainMenu()
        {
            LevelManager.LoadMainMenu();
            REvent_GameManagerWinOrLossToMainMenu.Dispatch();
        }

        private void OnRestartLevel()
        {
            LevelManager.LoadCurrentLevel();
            REvent_GameManagerWinOrLossToPlay.Dispatch();
        }
    }
}