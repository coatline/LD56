using UnityEngine;

public abstract class State
{
    protected readonly StateMachine machine;
    protected readonly Flemington flemington;

    public State(StateMachine machine, Flemington flemington)
    {
        this.machine = machine;
        this.flemington = flemington;
    }

    public virtual void Enter() { }
    public abstract void Update(float deltaTime);
    public virtual void DestinationReached() { }
    public virtual void Exit() { }
    public abstract string GetInspectText();
}