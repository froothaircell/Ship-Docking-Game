using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Mediators;
using CoreResources.Pool;
using GameResources.Events;
using UnityEngine;

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
        }
        
        public void OnEnterWin(REvent evt)
        {
            View.gameObject.SetActive(true);
            OnEnterMenu();
            View.nextLevelButton.onClick.AddListener(OnNextLevel);
            View.nextLevelButton.gameObject.SetActive(true);
        }

        public void OnEnterLoss(REvent evt)
        {
            View.gameObject.SetActive(true);
            OnEnterMenu();
            View.nextLevelButton.gameObject.SetActive(false);
        }
        
        public override void OnEnterMenu()
        {
            View.mainMenuButton.onClick.AddListener(OnMainMenu);
            View.restartLevelButton.onClick.AddListener(OnRestartLevel);

            View.mainMenuButton.gameObject.SetActive(true);
            View.restartLevelButton.gameObject.SetActive(true);
            View.winOrLossText.gameObject.SetActive(true);
        }

        public override void OnExitMenu()
        {
            View.RemoveAllListeners();
        }

        private void OnNextLevel()
        {
            REvent_GameManagerMainMenuToPlay.Dispatch();
        }

        private void OnMainMenu()
        {
            REvent_GameManagerPlayToMainMenu.Dispatch();
        }

        private void OnRestartLevel()
        {
            REvent_GameManagerMainMenuToPlay.Dispatch();
        }
    }
}