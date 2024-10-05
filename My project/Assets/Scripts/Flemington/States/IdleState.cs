using UnityEngine;
using System.Collections.Generic;

public class IdleState : State
{
    public IdleState(StateMachine machine, Flemington flemington) : base(machine, flemington)
    {

    }

    public override void DestinationReached()
    {
    }

    public override void Update(float deltaTime)
    {
    }

    protected override void Being()
    {
    }

    protected override void Quit()
    {
    }
}
