using CoreResources.Pool;
using CoreResources.Utils;

namespace CoreResources
{
    public class AppHandler : GenericSingleton<AppHandler>
    {
        public TypePool AppPool = new TypePool("AppPool");
    }
}