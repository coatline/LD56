using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    protected Dictionary<ItemType, int> itemToStored;
    List<Item> items;

    protected virtual void Awake()
    {
        itemToStored = new Dictionary<ItemType, int>();
        items = new List<Item>();
    }

    public virtual void AddItem(Item item)
    {
        if (itemToStored.ContainsKey(item.Type))
            itemToStored[item.Type]++;
        else
            itemToStored.Add(item.Type, 1);

        Village.I.DestroyItem(item);
    }

    public void SendItem(Item item) => items.Add(item);

    private void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];

            item.transform.position = Vector3.Lerp(item.transform.position, transform.position, Time.deltaTime);

            if (Vector2.Distance(item.transform.position, transform.position) < 0.5f)
            {
                AddItem(item);
                items.RemoveAt(i);
            }
        }
    }

    protected int GetStoredAmount(ItemType type)
    {
        if (itemToStored.ContainsKey(type) == false)
            itemToStored.Add(type, 0);
        return itemToStored[type];
    }
}