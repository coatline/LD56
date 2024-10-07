using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTask : Task
{
    public readonly Item ToGrab;
    float pickupTimer;

    public GrabTask(Item toGrab) : base(null, true)
    {
        this.ToGrab = toGrab;
    }

    public override void DoWork(float deltaTime)
    {
        pickupTimer += deltaTime;

        if (pickupTimer > 0.5f)
        {
            flemington.PickupItem(ToGrab);
            Complete();
        }
    }

    public override void Start(Flemington flemington)
    {
        base.Start(flemington);

        //Debug.Log($"Starting, Reserved = {ToGrab.Reserved}");

        if (ToGrab.Reserved)
            Cancel();
        else
            ToGrab.Reserved = true;
    }

    public override void Cancel()
    {
        //Debug.Log("Canceling and Reserved=false");

        ToGrab.Reserved = false;
        base.Cancel();
    }

    public override string GetTextString()
    {
        string str = $"Grabbing {ToGrab.name}";
        return str;
    }

    public override float MinDistance => 0.05f;
    public override Vector2 GetTargetPosition() => ToGrab.transform.position;
}
