using UnityEngine;
using System.Collections.Generic;

public class TaskState : State
{
    //public readonly Task Task;

    //public TaskState(Task task, Flemington flemington) : base(flemington)
    //{
    //    Task = task;

    //    Task.OnCompleted += FinishWithTask;
    //}

    //public override void Update(float deltaTime)
    //{
    //    // Are we at the site of work?
    //    if (flemington.AtPosition(Task.GetTargetPosition(), 0.05f) == false)
    //    {
    //        MoveState moveState = new MoveState(flemington, Task.GetTargetPosition, 1, this, this);
    //        flemington.StateMachine.SetState(moveState);
    //    }
    //    else
    //    {
    //        // Do work.
    //        Task.DoWork(flemington, deltaTime);
    //    }
    //}

    //public void FinishWithTask(Task t = null)
    //{
    //    Task.OnCompleted -= FinishWithTask;

    //    //DoneWithTask.Invoke();

    //    ToNextState();
    //}

    //public override string GetInspectText()
    //{
    //    string str = $"Doing {Task.GetTextString()}";

    //    return str;
    //}
}
