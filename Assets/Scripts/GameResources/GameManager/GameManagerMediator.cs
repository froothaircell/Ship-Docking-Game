using CoreResources.Mediators;
using GameResources.Events;
using GameResources.GameManager.States;

namespace GameResources.GameManager
{
    public class RGameManagerMediator : StateMachineMediator<RGameManagerMediator, RGameManagerStateMachine>
    {
        public override void Initialize()
        {
            // Start the state machine with the state for the main menu
            FSM.GoToState(RGameManagerStateContext.GetContext(typeof(GameManagerState_MainMenu)));
            REvent_GameStart.Dispatch();
            base.Initialize();
        }
    }
}