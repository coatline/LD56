using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : Task
{
    float idleStartTime;
    Vector2 destination;
    float idleDuration;
    float idleTimer;

    public IdleTask() : base(null, true)
    {
    }

    public override void DoWork(float deltaTime)
    {
        if (idleTimer > idleDuration)
            NewPosition();
        else
            idleTimer += deltaTime;
    }

    public override void Start(Flemington flemington)
    {
        base.Start(flemington);
        NewPosition();
        idleStartTime = TimeManager.I.SimulationTime;
    }

    void NewPosition()
    {
        destination = flemington.transform.position + C.GetRandVector(-1, 1f);
        idleDuration = Random.Range(1f, 4f);
        idleTimer = 0;
    }

    public override Task GetNextTask(Flemington flemington) => null;

    public override string GetTextString()
    {
        string str = $"Idle ({C.DisplayTimeFromSeconds((int)(TimeManager.I.SimulationTime - idleStartTime))})";
        return str;
    }

    public override Vector2 GetTargetPosition() => destination;
    public override float MinDistance => 0.05f;
}
