[System.Serializable]
public class ItemStack
{
    public ItemType Item;
    public int Count;

    public ItemStack(ItemType item, int count)
    {
        Item = item;
        Count = count;
    }

    public ItemStack(ItemStack copy)
    {
        Item = copy.Item;
        Count = copy.Count;
    }
}