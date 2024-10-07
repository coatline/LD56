using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverTask : Task
{
    readonly ItemType toDeliver;
    readonly ItemHolder destination;

    public DeliverTask(ItemType toDeliver, ItemHolder itemHolder, Job rootJob) : base(rootJob)
    {
        this.toDeliver = toDeliver;
        this.destination = itemHolder;
        itemHolder.ItemHolderDestroyed += DestinationDestroyed;
        NeededItems = new List<ItemType> { toDeliver };
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        flemington.StoreItem(destination);
        Complete();
    }

    void DestinationDestroyed(ItemHolder holder) => Cancel();

    public override void FixCanceled(Flemington flemington)
    {
        if (flemington.Carrying != null)
            flemington.DropItem();
        base.FixCanceled(flemington);
    }

    public override string GetTextString()
    {
        string str = $"Delivering {toDeliver.name} to {destination.name}";
        return str;
    }

    public override float MinDistance => 0.3f;
    public override Vector2 GetTargetPosition() => destination.transform.position;
}
