using UnityEngine;
using System.Collections.Generic;

public class IdleState : State
{
    //public IdleState(Flemington flemington) : base(flemington)
    //{
    //}

    //public override void Update(float deltaTime)
    //{
    //    if (elapsed > idleDuration)
    //    {
    //        float dir = Random.Range(-1f, 1f);
    //        Vector2 destination = flemington.Position + new Vector2(dir, 0);

    //        flemington.StateMachine.SetState(new MoveState(flemington, () => destination, 0.5f, this, this));

    //        idleDuration = Random.Range(.5f, 4f);
    //        elapsed = 0;
    //    }
    //    else
    //        elapsed += deltaTime;
    //}

    ////public override void Enter()
    ////{
    ////    idleStartTime = Time.time;
    ////}

    //public override string GetInspectText()
    //{
    //    return $"Idle ({C.DisplayTimeFromSeconds((int)(flemington.StateMachine.idleStartTime))})";
    //}
}
