using CoreResources.Utils.Singletons;
using GameResources;

namespace CoreResources.Utils.SaveData
{
    public class PlayerModel : GenericSingleton<PlayerModel>
    {
        private int _score;
        private int _level;

        public int Score => _score;
        public int Level => _level;

        public void UpdateStats()
        {
            AppHandler.SaveManager.GetPlayerInfo(ref _score, ref _level);
        }

        public void UpdateSaveData()
        {
            AppHandler.SaveManager.SetPlayerInfo(_score, _level);
        }

        public void Init()
        {
            UpdateStats();
        }
    }
}