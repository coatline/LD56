using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTask : Task
{
    public readonly Item ToGrab;
    float pickupTimer;

    public GrabTask(Job parentJob, Item toGrab) : base(parentJob)
    {
        this.ToGrab = toGrab;
        toGrab.Reserved = true;
    }

    public override void WorkOn(Flemington flemington, float deltaTime)
    {
        pickupTimer += deltaTime;

        if (pickupTimer > 0.5f)
        {
            flemington.PickupItem(ToGrab);
            Completed();
        }
    }

    public override void Cancel()
    {
        ToGrab.Reserved = false;
        base.Cancel();
    }

    public override string GetTextString()
    {
        string str = $"Grabbing: {ToGrab.name}";
        return base.GetTextString() + str;
    }

    public override Vector2 GetTargetPosition() => ToGrab.transform.position;
}
