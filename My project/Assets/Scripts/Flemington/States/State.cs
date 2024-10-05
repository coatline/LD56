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

    protected abstract void Being();
    public abstract void Update(float deltaTime);
    public abstract void DestinationReached();
    protected abstract void Quit();
}