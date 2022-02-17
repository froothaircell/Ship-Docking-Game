using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;
using CoreResources.Utils.ResourceLoader;
using CoreResources.Utils.SaveData;
using GameResources.GameManager;

namespace CoreResources
{
    public class AppHandler : GenericSingleton<AppHandler>
    {
        public static TypePool AppPool = new TypePool("AppPool");
        public static TypePool EventPool = new TypePool("EventPool");
        public static REventHandler EventHandler;
        public static AssetLoader AssetHandler;
        public static PlayerPrefsManager SaveManager;
        public static PlayerModel PlayerStats;
        // Essentially this mediator should run on itself
        // and not be accessed from here think about removing
        // this later
        public static GameManagerMediator GMMediator; 

        protected override void Awake()
        {
            Initialize();
            base.Awake();
        }

        private void Initialize()
        {
            // Make the core services available before starting the game related stuff
            EventHandler = REventHandler.Instance;
            PlayerStats = PlayerModel.Instance;
            SaveManager = PlayerPrefsManager.Instance;
            AssetHandler = AssetLoader.Instance;
            
            PlayerStats.Init();
            GMMediator = GameManagerMediator.Create();
        }
    }
}