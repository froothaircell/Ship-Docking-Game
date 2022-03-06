using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreResources.Utils;
using GameResources.Events;
using GameResources.Ship;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace GameResources.LevelAndScoreManagement
{
    public class LevelManager
    {
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

        public static async void LoadMainMenu()
        {
            AppHandler.PlayerStats.UpdateSaveData();
            SceneManager.LoadScene("EntryMenu");
            await Task.Delay(500);
            REvent_GameManagerPauseToMainMenu.Dispatch();
        }

        public static async void LoadNextLevel()
        {
            int nextLevel = FindNextLevel();
            AppHandler.PlayerStats.UpdateAndSave(-1, nextLevel);
            SceneManager.LoadScene("Level" + nextLevel);
            await Task.Delay(500);
            REvent_LevelStart.Dispatch();
        }
        
        public static async void LoadCurrentLevel()
        {
            AppHandler.PlayerStats.UpdateSaveData();
            SceneManager.LoadScene("Level" + AppHandler.PlayerStats.Level);
            await Task.Delay(500);
            REvent_LevelStart.Dispatch();
        }

        public static async void LoadArbitraryLevel(int level)
        {
            string levelStr = "Level" + level;
            if (Application.CanStreamedLevelBeLoaded(levelStr))
            {
                AppHandler.PlayerStats.UpdateAndSave(-1, level);
                SceneManager.LoadScene(levelStr);
                await Task.Delay(500);
                REvent_LevelStart.Dispatch();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Level {level} does not exist!");
            }
        }

        private static int FindNextLevel()
        {
            int nextLevel = AppHandler.PlayerStats.Level + 1;
            if(Application.CanStreamedLevelBeLoaded("Level" + nextLevel))
            {
                return nextLevel;
            }
            else
            {
                return 1;
            }
        }
    }
}