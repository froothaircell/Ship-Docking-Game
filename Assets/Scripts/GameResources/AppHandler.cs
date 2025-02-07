using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Jobs;
using CoreResources.Utils.ResourceLoader;
using CoreResources.Utils.SaveData;
using CoreResources.Utils.Singletons;
using GameResources.GameManager;
using GameResources.InputManagement;
using GameResources.LevelAndScoreManagement;
using GameResources.Menus.MainMenu;
using GameResources.Menus.PauseAndHudMenu;
using GameResources.Menus.WinOrLossMenu;
using GameResources.Ship;
using UnityEngine;

namespace GameResources
{
    public class AppHandler : MonobehaviorSingleton<AppHandler>
    {
        public static TypePool AppPool = new TypePool("AppPool");
        public static TypePool EventPool = new TypePool("EventPool");
        public static TypePool JobPool = new TypePool("Job Pool");
        public static REventHandler EventHandler;
        public static AssetLoader AssetHandler;
        public static PlayerModel PlayerStats;
        public static PlayerPrefsManager SaveManager;
        public static RInputManager InputHandler;
        public static JobManager JobHandler;
        // Essentially this mediator should run on itself
        // and not be accessed from here think about removing
        // this later
        public static RGameManagerMediator GMMediator;
        public static RMainMenuMediator MainMenuMediator;
        public static RPauseAndHudMenuMediator PauseAndHudMenuMediator;
        public static RWinOrLossMenuMediator WinOrLossMenuMediator;
        public static ShipPoolManager ShipPoolHandler;
        public static ShipSpawnManager ShipSpawnHandler;
        public static ScoreManager ScoreHandler;
        public static LevelManager LevelHandler;

        protected override void Awake()
        {
            InitSingleton();
            if (GetInstanceID() == Instance.GetInstanceID())
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            // Check for windows build and set screen size
#if UNITY_STANDALONE_WIN
            Screen.SetResolution(564, 960, false);
            Screen.fullScreen = false;
#endif

            // Make the core services available before starting the game related stuff
            EventHandler = REventHandler.SetInstanceType<REventHandler>();
            SaveManager = PlayerPrefsManager.SetInstanceType<PlayerPrefsManager>();
            PlayerStats = PlayerModel.SetInstanceType<PlayerModel>();
            AssetHandler = AssetLoader.SetInstanceType<AssetLoader>();
            InputHandler = RInputManager.Instance;
            MainMenuMediator = RMainMenuMediator.Instance;
            PauseAndHudMenuMediator = RPauseAndHudMenuMediator.Instance;
            WinOrLossMenuMediator = RWinOrLossMenuMediator.Instance;

            JobHandler = JobManager.SetInstanceType<JobManager>();
            InitializeMenus();
            
            ShipPoolHandler = ShipPoolManager.Instance;
            ShipSpawnHandler = ShipSpawnManager.SetInstanceType<ShipSpawnManager>();
            ScoreHandler = ScoreManager.SetInstanceType<ScoreManager>();
            LevelHandler = LevelManager.SetInstanceType<LevelManager>();
            
            // Run these at the end so that relevant scripts can run their callbacks on the first state
            GMMediator = RGameManagerMediator.Create();
            GMMediator.Initialize();
        }

        private void InitializeMenus()
        {
            MainMenuMediator.SubscribeToEvents();
            PauseAndHudMenuMediator.SubscribeToEvents();
            WinOrLossMenuMediator.SubscribeToEvents();
        }
    }
}