using UnityEngine;


public class DeliverJob : Job
{
    readonly int maxDeliversAtOnce;
    readonly ItemStack toDeliver;
    readonly ItemHolder storage;
    int deliverTasks;

    public DeliverJob(ItemStack toDeliver, ItemHolder storage, int maxDeliversAtOnce)
    {
        this.toDeliver = toDeliver;
        this.storage = storage;
        this.maxDeliversAtOnce = maxDeliversAtOnce;
        this.storage.ItemHolderDestroyed += StorageDestroyed;

        for (int i = 0; i < toDeliver.Count; i++)
        {
            //if (i >= maxDeliversAtOnce) break;
            //deliverTasks++;
            CreateTask(new DeliverTask(toDeliver.Item, storage, this));
        }
    }

    void StorageDestroyed(ItemHolder holder)
    {
        storage.ItemHolderDestroyed -= StorageDestroyed;
        Cancel();
    }

    protected override void Complete()
    {
        storage.ItemHolderDestroyed -= StorageDestroyed;
        base.Complete();
    }

    protected override void TaskCompleted(Task task)
    {
        base.TaskCompleted(task);
        if (toDeliver.Count <= 0)
            Complete();
        //else if (deliverTasks < toDeliver.Count && availableTasks.Count < maxDeliversAtOnce)
        //{
        //    deliverTasks++;
        //    availableTasks.Add(new DeliverTask(toDeliver.Item, storage, this));
        //}
    }
}