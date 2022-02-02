using System;
using CoreResources.Pool;

namespace CoreResources.StateMachine
{
    public class StateContext : Poolable
    {
        public Type OpenState { get; private set; }
        
        protected static TStateContext Get<TStateContext>(Type openState) where TStateContext : StateContext, new()
        {
            TStateContext context = AppHandler.Instance.AppPool.Get<TStateContext>();
            context.OpenState = openState;

            return context;
        }

        public static void UpdateOpenState(StateContext context, Type openState)
        {
            context.OpenState = openState;
        }
        
        protected override void OnSpawn()
        {
            
        }

        protected override void OnDespawn()
        {
            OpenState = null;
        }
    }
}