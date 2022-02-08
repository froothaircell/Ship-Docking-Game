using CoreResources.Pool;

namespace CoreResources.Handlers.EventHandler
{
    // Class is pooled and can have its own implementation for invoke
    // Correction : definition of the invoke/dispatch function is necessary
    public class REvent : Poolable
    {
        public static T Get<T>() where T : REvent, new()
        {
            var temp = AppHandler.EventPool.Get<T>();
            return temp;
        }

        protected override void OnSpawn()
        {
            
        }

        protected override void OnDespawn()
        {
            
        }
    }
}