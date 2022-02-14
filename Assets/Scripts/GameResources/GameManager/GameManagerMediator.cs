using CoreResources.Mediators;
using GameResources.GameManager.States;

namespace GameResources.GameManager
{
    public class GameManagerMediator : StateMachineMediator<GameManagerMediator, GameManagerStateMachine>
    {
        protected override void Initialize()
        {
            // Start the state machine with the state for the main menu
            FSM.GoToState(GameManagerStateContext.GetContext(typeof(GameManagerState_MainMenu)));
            base.Initialize();
        }
    }
}