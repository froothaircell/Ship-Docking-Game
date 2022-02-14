using System;
using CoreResources.StateMachine;
using CoreResources.Utils.SaveData;
using UnityEngine;

namespace GameResources.GameManager
{
    public class GameManagerStateContext : StateContext
    {
        public static GameManagerStateContext GetContext(Type defaultType) 
        {
            GameManagerStateContext context = StateContext.Get<GameManagerStateContext>(defaultType);
            return context;
        }
    }
}