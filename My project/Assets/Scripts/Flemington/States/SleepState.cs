using UnityEngine;
using System.Collections.Generic;

public class SleepState : State
{

    //public SleepState(Flemington flemington) : base(flemington)
    //{
    //}

    //public override void Update(float deltaTime)
    //{
    //    // Are we still tired?
    //    if (flemington.NeedToBehavior[NeedType.Sleep].Amount < 1)
    //    {
    //        // Are we at our house?
    //        if (flemington.AtPosition(flemington.House.transform.position, 0.1f))
    //        {
    //            // Sleep
    //            flemington.NeedToBehavior[NeedType.Sleep].ModifyAmount(deltaTime);
    //            flemington.DoingBubble.ShowSleeping();
    //        }
    //        else
    //        {
    //            // Go home.
    //            flemington.StateMachine.SetState(new MoveState(flemington, () => flemington.House.transform.position, 1f, this, this));
    //        }

    //    }
    //    else
    //    {
    //        // New State
    //        ToNextState();
    //    }
    //}

    //public override void Exit()
    //{
    //    flemington.DoingBubble.StopShowing();
    //}

    //public override string GetInspectText()
    //{
    //    string str = "Sleeping";

    //    //if (Task == null)
    //    //    str += "nothing";
    //    //else
    //    //    str += $"{Task.GetTextString()}";

    //    return str;
    //}
}
