using UnityEngine;


public class DeliverJob : Job
{
    public DeliverJob(ItemStack toDeliver, ItemHolder storage)
    {
        // TODO: maybe not flood the market with these (only some at a time)

        for (int i = 0; i < toDeliver.Count; i++)
        {
            CreateTask(new DeliverTask(this, toDeliver.Item, storage));
        }
    }
}