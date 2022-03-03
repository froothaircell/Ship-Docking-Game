using CoreResources.StateMachine;
using UnityEngine;
using System;

public class SampleStateMachine : StateMachineHistory<SampleStateMachine, SampleState, SampleStateContext>
{
    
}

public class SampleState : StateHistory<SampleStateMachine, SampleState, SampleStateContext>
{
    
}

public class SampleState1 : SampleState
{
    
}

public class SampleState2 : SampleState
{
    
}

public class SampleStateContext : StateContext
{
    public static SampleStateContext GetContext(Type state)
    {
        SampleStateContext context = StateContext.Get<SampleStateContext>(state);
        return context;
    }
    
}

public class StateMachineTest : MonoBehaviour
{
    SampleStateMachine someStateMachine = new SampleStateMachine();

    private void Start()
    {
        someStateMachine.GoToState(SampleStateContext.GetContext(typeof(SampleState1)));
        someStateMachine.GoToState<SampleState2>();
        someStateMachine.GoToState(SampleStateContext.GetContext(typeof(SampleState2)));
        someStateMachine.GoToState<SampleState1>();
        someStateMachine.GoToPreviousState(2);
        someStateMachine.GoToPreviousState();
        someStateMachine.GoToNextState(3);
        someStateMachine.GoToStateNonHistorically(SampleStateContext.GetContext(typeof(SampleState2)));
        someStateMachine.GoToState<SampleState2>();
        someStateMachine.GoToState<SampleState1>();
        someStateMachine.GoToState<SampleState2>();
        someStateMachine.GoToState<SampleState2>();
        someStateMachine.GoToState<SampleState1>();
        someStateMachine.GoToState<SampleState2>();
        someStateMachine.GoToPreviousState(3);
        someStateMachine.GoToState<SampleState1>();
        someStateMachine.GoToPreviousState(4);
    }
}