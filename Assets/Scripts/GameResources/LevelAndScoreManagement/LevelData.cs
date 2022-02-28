using UnityEngine;

namespace GameResources.LevelAndScoreManagement
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public int Level;
        public int ScoutBoatQuantity;
        public int FisherBoatQuantity;
        public int SpeedBoatQuantity;
        public int JetSkiQuantity;
    }
}