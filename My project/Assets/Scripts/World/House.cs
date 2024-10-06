

public class House : Building
{
    public Flemington Owner { get; set; }

    protected override void Completed()
    {
        base.Completed();
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