using UnityEngine;
using System.Collections.Generic;

public class WorkState : State
{
    public WorkState(StateMachine machine, Flemington flemington) : base(machine, flemington)
    {

    }

    public override void DestinationReached()
    {
        if (machine.Task == null)
            return;

        machine.Task.WorkOn(Time.deltaTime);
    }

    public override void Update(float deltaTime)
    {
        if (machine.Task == null)
            return;

        flemington.SetDestination(machine.Task.TargetPosition);
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override string GetInspectText()
    {
        string str = "Task: ";

        if (machine.Task == null)
            str += "null";
        else
            str += $"{machine.Task.GetTextString()}";

        return str;
    }
}
