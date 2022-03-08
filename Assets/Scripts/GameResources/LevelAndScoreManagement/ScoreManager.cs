using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Singletons;
using GameResources.Events;

namespace GameResources.LevelAndScoreManagement
{
    public class ScoreManager : InitializableGenericSingleton<ScoreManager>
    {
        private int _dockedShips;
        private int _destroyedShips;
        private int _totalShips;
        
        protected override void InitSingleton()
        {
            base.InitSingleton();
            
            ResetValues();
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnReset, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnReset, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_ShipDocked>(IncrementDockedShips, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_ShipDestroyed>(IncrementDestroyedShips, _disposables);
        }

        private void OnReset(REvent evt)
        {
            ResetValues();
        }

        private void ResetValues()
        {
            _dockedShips = 0;
            _destroyedShips = 0;
            _totalShips = GetTotalShipCount();
        }

        private int GetTotalShipCount()
        {
            return LevelManager.TotalShipsForLevel();
        }

        
        private void IncrementDockedShips(REvent evt)
        {
            if (_dockedShips < _totalShips)
            {
                _dockedShips++;
                int dockedPerc = (_dockedShips * 100) / _totalShips;
                REvent_DisplayScore.Dispatch(dockedPerc);
                // Won the level
                if (dockedPerc >= 75)
                {
                    // this is just a sample equation, subject to change later
                    int levelScore = (_dockedShips - _destroyedShips) * 10;
                    AppHandler.PlayerStats.UpdateScoreAndLevel(levelScore, -1);
                    REvent_GameManagerPlayToWin.Dispatch();
                }
            }
        }

        private void IncrementDestroyedShips(REvent evt)
        {
            if (_destroyedShips < _totalShips)
            {
                _destroyedShips++;
                int destroyedPerc = (_destroyedShips * 100) / _totalShips;
                // Lost the level
                if (destroyedPerc > 25)
                {
                    REvent_GameManagerPlayToLoss.Dispatch();
                }
            }
        }
    }
}