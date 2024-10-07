

public class House : Building
{
    public Flemington Owner { get; private set; }

    public void SetOwner(Flemington owner)
    {
        Owner = owner;
        Owner.Died += OwnerDied;
    }

    void OwnerDied(Flemington owner)
    {
        Owner.Died -= OwnerDied;
        Owner = null;
        Village.I.HouseAvailable(this);
    }

    public override string Name
    {
        get
        {
            string str;

            if (built)
                if (Owner != null)
                    str = $"{Owner.name}'s House";
                else
                    str = $"Empty House";
            else
                str = "Unfinished House";

            return str;
        }
    }

    //public override string Content
    //{
    //    get
    //    {
    //        string str = "";

    //        if (built)
    //            str = "Completed";
    //        else
    //        {
    //            str = $"Required Materials : \n";
    //            for (int i = 0; i < buildingMaterials.Count; i++)
    //                str += $"{buildingMaterials.Item.name} ({itemToStored[buildingMaterials.Item]}/{buildingMaterials.Count})";
    //        }

    //        return str;
    //    }
    //}
}