using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : Task
{
    readonly Flemington flemington;
    Vector2 destination;
    float idleDuration;
    float idleTimer;

    public IdleTask(Flemington flemington) : base()
    {
        this.flemington = flemington;
        NewPosition();
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        if (idleTimer > idleDuration)
            NewPosition();
        else
            idleTimer += deltaTime;
    }

    void NewPosition()
    {
        float dir = Random.Range(-1f, 1f);
        destination = flemington.Position + new Vector2(dir, 0);
        idleDuration = Random.Range(.5f, 4f);
        idleTimer = 0;
    }

    public override Task GetNextTask(Flemington flemington) => null;

    public override string GetTextString()
    {
        string str = $"Idle ({Time.time - flemington.StateMachine.IdleStartTime})";
        return base.GetTextString() + str;
    }

    public override Vector2 GetTargetPosition() => destination;
    public override float MinDistance => 0.05f;
}
