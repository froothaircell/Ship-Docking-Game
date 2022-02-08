using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;

namespace CoreResources
{
    public class AppHandler : GenericSingleton<AppHandler>
    {
        public static TypePool AppPool = new TypePool("AppPool");
        public static TypePool EventPool = new TypePool("EventPool");
        public static REventHandler EventHandler;

        protected override void Awake()
        {
            InitEventHandler();
            base.Awake();
        }

        private void InitEventHandler()
        {
            EventHandler = REventHandler.Instance;
        }
    }
}