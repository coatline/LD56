using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTask : Task
{
    public readonly Item ToGrab;
    float pickupTimer;

    public GrabTask(Item toGrab) : base()
    {
        this.ToGrab = toGrab;
        toGrab.Reserved = true;
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        pickupTimer += deltaTime;

        if (pickupTimer > 0.5f)
        {
            flemington.PickupItem(ToGrab);
            Complete();
        }
    }

    public override void Cancel()
    {
        Debug.Log($"Canceled ");
        ToGrab.Reserved = false;
        base.Cancel();
    }

    public override string GetTextString()
    {
        string str = $"Grabbing: {ToGrab.name}";
        return base.GetTextString() + str;
    }

    public override float MinDistance => 0.05f;
    public override Vector2 GetTargetPosition() => ToGrab.transform.position;
}
