using CoreResources.Pool;

namespace CoreResources.Handlers
{
    // Class is pooled and can have its own implementation for invoke
    public class REvent : Poolable
    {
        public static T Get<T>() where T : REvent, new()
        {
            var temp = AppHandler.Instance.EventPool.Get<T>();
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