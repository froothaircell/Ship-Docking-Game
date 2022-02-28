using System.Collections.Generic;
using System.Threading.Tasks;
using CoreResources.Handlers.EventHandler;
using GameResources.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace GameResources.LevelAndScoreManagement
{
    public class LevelManager
    {
        public static void AccessLevelData(ref List<int> shipQuantities)
        {
            int CurrentLevel = AppHandler.PlayerStats.Level;
            LevelData lvlData = ScanForLevel(CurrentLevel);
            shipQuantities = new List<int>()
            {
                lvlData.ScoutBoatQuantity,
                lvlData.FisherBoatQuantity,
                lvlData.SpeedBoatQuantity,
                lvlData.JetSkiQuantity
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
            REvent_GameManagerPlayToMainMenu.Dispatch();
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