using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverTask : Task
{
    readonly ItemType toDeliver;
    readonly ItemHolder destination;

    public DeliverTask(Job parentJob, ItemType typeToPickup, ItemHolder itemHolder) : base(parentJob)
    {
        this.toDeliver = typeToPickup;
        // TODO: reserve item
    }

    public override void WorkOn(Flemington flemington, float deltaTime)
    {
    }

    public override string GetTextString()
    {
        string str = $"Delivering: {toDeliver.name}";
        return base.GetTextString() + str;
    }

    public override Vector2 TargetPosition
    {
        get
        {
            return destination.transform.position;
        }
    }
}
