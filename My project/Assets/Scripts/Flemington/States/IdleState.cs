using UnityEngine;
using System.Collections.Generic;

public class IdleState : State
{
    float idleStartTime;
    float idleDuration;
    float elapsed;

    public IdleState(StateMachine machine, Flemington flemington) : base(machine, flemington)
    {
        idleDuration = Random.Range(.5f, 4f);
    }

    public override void Update(float deltaTime)
    {
        if (elapsed > idleDuration)
        {
            float dir = Random.Range(-1f, 1f);
            flemington.SetDestination(flemington.transform.position + new Vector3(dir * 3, 0));
            idleDuration = Random.Range(.5f, 4f);
            elapsed = 0;
        }
        else
            elapsed += deltaTime;
    }

    public override void Enter()
    {
        idleStartTime = Time.time;
    }

    public override string GetInspectText()
    {
        return $"Idle for {C.DisplayTimeFromSeconds((int)(Time.time - idleStartTime))}";
    }
}
