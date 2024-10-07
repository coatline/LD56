using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public event System.Action<ItemHolder> ItemHolderDestroyed;

    protected Dictionary<ItemType, int> itemToStored;
    List<Item> itemToAnimate;

    protected virtual void Awake()
    {
        itemToStored = new Dictionary<ItemType, int>();
        itemToAnimate = new List<Item>();
    }

    public virtual void AddItem(Item item)
    {
        if (itemToStored.ContainsKey(item.Type))
            itemToStored[item.Type]++;
        else
            itemToStored.Add(item.Type, 1);
    }

    public void SendItem(Item item)
    {
        AddItem(item);
        itemToAnimate.Add(item);
    }

    private void Update()
    {
        for (int i = 0; i < itemToAnimate.Count; i++)
        {
            Item item = itemToAnimate[i];

            item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, Time.deltaTime);

            if (Vector2.Distance(item.transform.position, transform.position) < 0.05f)
            {
                Village.I.DestroyItem(item);
                itemToAnimate.RemoveAt(i);
            }
        }
    }

    protected int GetStoredAmount(ItemType type)
    {
        if (itemToStored.ContainsKey(type) == false)
            itemToStored.Add(type, 0);
        return itemToStored[type];
    }

    protected virtual void OnDestroy()
    {
        ItemHolderDestroyed?.Invoke(this);
    }
}