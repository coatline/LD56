using UnityEngine;
using System.Collections.Generic;

public class TaskState : State
{
    public readonly Task Task;

    public TaskState(Task task, Flemington flemington) : base(flemington)
    {
        Task = task;

        Task.OnCompleted += FinishWithTask;
    }

    public override void Update(float deltaTime)
    {
        // Are we at the site of work?
        if (flemington.AtPosition(Task.GetTargetPosition(), 0.05f) == false)
        {
            MoveState moveState = new MoveState(flemington, Task.GetTargetPosition, 1, this, this);
            flemington.StateMachine.SetState(moveState);
        }
        else
        {
            // We are in position to do the task. This is us doing it.
            // i.e. crafting something at a workplace, researching, harvesting, cutting plant

            // Do work.
            Task.WorkOn(flemington, deltaTime);
        }
    }

    public void FinishWithTask(Task t = null)
    {
        Task.OnCompleted -= FinishWithTask;

        //DoneWithTask.Invoke();

        ToNextState();
    }

    public override string GetInspectText()
    {
        string str = $"Doing {Task.GetTextString()}";

        return str;
    }
}
