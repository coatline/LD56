using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTask : Task
{
    readonly Item toGrab;
    float pickupTimer;

    public GrabTask(Job parentJob, Item toGrab) : base(parentJob)
    {
        this.toGrab = toGrab;
        // TODO: reserve item
    }

    public override void WorkOn(Flemington flemington, float deltaTime)
    {
        pickupTimer += deltaTime;

        if (pickupTimer > 0.5f)
            Completed();
    }

    public override string GetTextString()
    {
        string str = $"Grabbing: {toGrab.name}";
        return base.GetTextString() + str;
    }

    public override Vector2 TargetPosition => toGrab.transform.position;
}
