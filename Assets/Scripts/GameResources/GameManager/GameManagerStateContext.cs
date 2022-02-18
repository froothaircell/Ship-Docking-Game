using System;
using CoreResources.StateMachine;

namespace GameResources.GameManager
{
    public class RGameManagerStateContext : StateContext
    {
        public static RGameManagerStateContext GetContext(Type defaultType) 
        {
            RGameManagerStateContext context = StateContext.Get<RGameManagerStateContext>(defaultType);
            return context;
        }
    }
}