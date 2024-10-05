using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    readonly Flemington flemington;
    Dictionary<StateType, State> stateToJohn;
    State currentState;

    public StateMachine(Flemington flemington)
    {
        this.flemington = flemington;

        stateToJohn = new Dictionary<StateType, State>()
        {
            (StateType.Idle, new IdleState()),

        };
        currentState = 
    }

    public void Update(float deltaTime)
    {
        Brain();
        currentState.Update(deltaTime);
    }

    void Brain()
    {

    }

    public void DestinationReached()
    {
        currentState.DestinationReached();
    }

    public enum StateType
    {
        Idle,
        Working,
        Eating,
        Sleeping,
        Playing,
        Talking
    }
}