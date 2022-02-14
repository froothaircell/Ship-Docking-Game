namespace CoreResources.Utils.SaveData
{
    public class PlayerModel : GenericSingleton<PlayerModel>
    {
        private int _score;
        private int _level;

        public void UpdateStats()
        {
            AppHandler.SaveManager.GetPlayerInfo(ref _score, ref _level);
        }

        public void UpdateSaveData()
        {
            AppHandler.SaveManager.SetPlayerInfo(_score, _level);
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateStats();
        }
    }
}