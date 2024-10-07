using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverTask : Task
{
    readonly ItemType toDeliver;
    readonly ItemHolder destination;

    public DeliverTask(ItemType toDeliver, ItemHolder itemHolder, Job rootJob) : base(rootJob, rootJob == null)
    {
        this.toDeliver = toDeliver;
        this.destination = itemHolder;
        itemHolder.ItemHolderDestroyed += DestinationDestroyed;
        NeededItems = new List<ItemType> { toDeliver };
    }

    public override void DoWork(float deltaTime)
    {
        flemington.StoreItem(destination);
        Complete();
    }

    void DestinationDestroyed(ItemHolder holder) => Cancel();

    public override void Cancel()
    {
        if (flemington != null)
            if (flemington.Carrying != null)
                flemington.DropItem();

        base.Cancel();
    }

    public override void Finish()
    {
        destination.ItemHolderDestroyed -= DestinationDestroyed;
        base.Finish();
    }

    public override string GetTextString()
    {
        string str = $"Delivering {toDeliver.name} to {destination.name}";
        return str;
    }

    public override float MinDistance => 0.3f;
    public override Vector2 GetTargetPosition() => destination.transform.position;
}
