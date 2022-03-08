using CoreResources.Utils.Singletons;
using GameResources;
using GameResources.Events;

namespace CoreResources.Utils.SaveData
{
    public class PlayerModel : InitializableGenericSingleton<PlayerModel>
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

        protected override void InitSingleton()
        {
            base.InitSingleton();
            UpdateStats();
            if (_level == 0)
            {
                _level = 1;
                _score = 0;
                UpdateSaveData();
            }
        }

        protected override void CleanSingleton()
        {
            base.CleanSingleton();
            UpdateSaveData();
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