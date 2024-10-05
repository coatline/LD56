using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Task Task { get; private set; }

    readonly Flemington flemington;
    Dictionary<StateType, State> typeToState;
    StateType currentType;
    State currentState;

    public StateMachine(Flemington flemington)
    {
        this.flemington = flemington;

        typeToState = new Dictionary<StateType, State>()
        {
            { StateType.Idle, new IdleState(this, flemington) },
            { StateType.Working, new WorkState(this, flemington) }
        };

        currentType = StateType.Sleeping;
        TrySwitchStateTo(StateType.Idle);
    }

    public void Update(float deltaTime)
    {
        Brain();
        currentState.Update(deltaTime);
    }

    void TrySwitchStateTo(StateType type)
    {
        if (currentType == type) return;

        if (currentState != null)
            currentState.Exit();

        currentType = type;
        currentState = typeToState[type];
        currentState.Enter();
    }

    void Brain()
    {
        // If we don't have any other needs to attend to...
        // ...
        // ...
        // Do the closest task

        if (Task == null)
            SetTask(Village.I.GetClosestTask(flemington.transform.position));

        if (Task != null)
            TrySwitchStateTo(StateType.Working);
        else
        {
            // Attend to our needs (playing, socializing)
            TrySwitchStateTo(StateType.Idle);
        }
    }

    void SetTask(Task task)
    {
        if (Task != null)
            Task.OnCompleted -= TaskCompleted;

        Task = task;

        if (Task != null)
            Task.OnCompleted += TaskCompleted;
    }

    void TaskCompleted(Task task)
    {
        //Debug.Log("Task Completed");
        SetTask(null);
        //Debug.Log($"Task {Task}");
        //Debug.Log($"Task {Task}");
    }

    public void DestinationReached()
    {
        currentState.DestinationReached();
    }

    public string GetStateText() => currentState.GetInspectText();

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