using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreResources.Utils;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.Ship;
using UnityEngine;


namespace GameResources.LevelAndScoreManagement
{
    // This class manages the access of level based data and loading and unloading of levels
    public class LevelManager : InitializableGenericSingleton<LevelManager>
    {
        private GameObject _currentLevel;

        protected override void CleanSingleton()
        {
            DestroyCurrentLevel();
            base.CleanSingleton();
        }

        public static int TotalShipsForLevel()
        {
            int tmp = 0;
            Dictionary<ShipTypes, int> totalShips = new Dictionary<ShipTypes, int>();
            AccessLevelData(ref totalShips);
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                tmp += totalShips[shipType];
            }

            return tmp;
        }
        
        public static void AccessLevelData(ref Dictionary<ShipTypes, int> shipQuantities)
        {
            int CurrentLevel = AppHandler.PlayerStats.Level;
            LevelData lvlData = ScanForLevel(CurrentLevel);
            shipQuantities = new Dictionary<ShipTypes, int>()
            {
                {ShipTypes.ScoutBoat, lvlData.ScoutBoatQuantity},
                {ShipTypes.FisherBoat, lvlData.FisherBoatQuantity},
                {ShipTypes.SpeedBoat, lvlData.SpeedBoatQuantity},
                {ShipTypes.JetSki, lvlData.JetSkiQuantity}
            };
        }

        private static LevelData ScanForLevel(int currentLevel)
        {
            return AppHandler.AssetHandler.LoadAsset<LevelData>("Level " + currentLevel);
        }

        public void LoadMainMenu()
        {
            AppHandler.PlayerStats.UpdateSaveData();
            if(_currentLevel != null)
                _currentLevel.SetActive(false);
            REvent_GameManagerPauseToMainMenu.Dispatch();
        }

        public void LoadNextLevel()
        {
            LoadArbitraryLevel(AppHandler.PlayerStats.Level + 1, REvent_LevelStart.Dispatch);
        }
        
        public void LoadCurrentLevel()
        {
            LoadArbitraryLevel(AppHandler.PlayerStats.Level, REvent_LevelStart.Dispatch);
        }

        private void LoadArbitraryLevel(int level, Action onLoadComplete = null)
        {
            string levelStr = FindLevel("Level" + level);
            if (AppHandler.AssetHandler.HasAsset(levelStr))
            {
                AppHandler.PlayerStats.UpdateAndSave(-1, level);
                AssignAndLoadLevel(levelStr, onLoadComplete);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Level {level} does not exist!");
            }
        }

        private string FindLevel(string levelName)
        {
            if (AppHandler.AssetHandler.HasAsset(levelName))
            {
                return levelName;
            }

            return "Level" + 1;
        }

        private async void AssignAndLoadLevel(string levelName, Action onLoadComplete = null)
        {
            if (_currentLevel != null)
            {
                if (_currentLevel.name == levelName)
                {
                    _currentLevel.SetActive(true);
                    onLoadComplete?.Invoke();
                    return;
                }

                DestroyCurrentLevel();
            }
            _currentLevel = AppHandler.AssetHandler.LoadAsset<GameObject>(levelName);
            _currentLevel = GameObject.Instantiate(_currentLevel);
            await Task.Delay(200);
            _currentLevel.name = levelName;
            _currentLevel.SetActive(true);
            onLoadComplete?.Invoke();
        }

        private void DestroyCurrentLevel()
        {
            if (_currentLevel != null)
            {
                GameObject.Destroy(_currentLevel);
                _currentLevel = null;
            }
        }
    }
}