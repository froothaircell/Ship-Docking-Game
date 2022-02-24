using System.Collections.Generic;
using System.IO;
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.LevelManagement
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
    }
}