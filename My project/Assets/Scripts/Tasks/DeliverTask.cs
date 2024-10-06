using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverTask : Task
{
    readonly ItemType toDeliver;
    readonly ItemHolder destination;

    public DeliverTask(Job parentJob, ItemType toDeliver, ItemHolder itemHolder) : base(parentJob)
    {
        this.toDeliver = toDeliver;
        this.destination = itemHolder;
        NeededItems = new List<ItemType> { toDeliver };
    }

    public override void WorkOn(Flemington flemington, float deltaTime)
    {
        flemington.StoreItem(destination);
        Completed();
    }

    public override string GetTextString()
    {
        string str = $"Delivering: {toDeliver.name}";
        return base.GetTextString() + str;
    }

    public override Vector2 GetTargetPosition() => destination.transform.position;
}
