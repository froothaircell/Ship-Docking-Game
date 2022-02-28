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
            if (_level == 0)
            {
                _level = 1;
                _score = 0;
                UpdateSaveData();
            }
        }

        // Just set the value of a parameter to -1 if we don't need to add it
        public void UpdateScoreAndLevel(int score, int level)
        {
            // score gets added
            if (score > 0)
            {
                _score += score;
            }

            if (level > 0)
            {
                _level = level;
            }
        }

        public void UpdateAndSave(int score, int level)
        {
            UpdateScoreAndLevel(score, level);
            UpdateSaveData();
        }

        public void ResetStats()
        {
            _score = 0;
            _level = 1;
            UpdateSaveData();
        }
    }
}