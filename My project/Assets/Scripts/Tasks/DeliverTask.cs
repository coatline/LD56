using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverTask : Task
{
    readonly ItemType toDeliver;
    readonly ItemHolder destination;


    public DeliverTask(ItemType toDeliver, ItemHolder itemHolder) : base()
    {
        this.toDeliver = toDeliver;
        this.destination = itemHolder;
        NeededItems = new List<ItemType> { toDeliver };
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        flemington.StoreItem(destination);
        Complete();
    }

    public override string GetTextString()
    {
        string str = $"Delivering: {toDeliver.name}";
        return base.GetTextString() + str;
    }

    public override float MinDistance => 0.05f;
    public override Vector2 GetTargetPosition() => destination.transform.position;
}
