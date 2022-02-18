using System;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;
using CoreResources.Utils.ResourceLoader;
using CoreResources.Utils.SaveData;
using GameResources.GameManager;
using GameResources.Menus.MainMenu;
using GameResources.Menus.PauseAndHudMenu;
using GameResources.Menus.WinOrLossMenu;

namespace GameResources
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
        public static RGameManagerMediator GMMediator;
        public static RMainMenuMediator MainMenuMediator;
        public static RPauseAndHudMenuMediator PauseAndHudMenuMediator;
        public static RWinOrLossMenuMediator WinOrLossMenuMediator;

        private void Awake()
        {
            InitSingleton();
            Initialize();
        }

        private void Initialize()
        {
            // Make the core services available before starting the game related stuff
            EventHandler = REventHandler.Instance;
            PlayerStats = PlayerModel.Instance;
            SaveManager = PlayerPrefsManager.Instance;
            AssetHandler = AssetLoader.Instance;
            MainMenuMediator = RMainMenuMediator.Instance;
            PauseAndHudMenuMediator = RPauseAndHudMenuMediator.Instance;
            WinOrLossMenuMediator = RWinOrLossMenuMediator.Instance;
            
            
            PlayerStats.Init();
            InitializeMenus();
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