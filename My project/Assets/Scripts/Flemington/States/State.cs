using UnityEngine;

public abstract class State
{
    protected readonly Flemington flemington;

    public State NextState { get; protected set; }
    public State RootState { get; private set; }

    public State(Flemington flemington, State nextState = null, State rootState = null)
    {
        this.flemington = flemington;

        if (rootState == null)
            rootState = this;

        RootState = rootState;
        NextState = nextState;
    }

    protected void ToNextState() => flemington.StateMachine.SetState(NextState);

    public abstract void Update(float deltaTime);
    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract string GetInspectText();
}