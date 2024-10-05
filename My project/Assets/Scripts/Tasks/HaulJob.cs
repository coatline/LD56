using UnityEngine;


public class HaulJob : Job
{
    readonly Item toHaul;

    public HaulJob(Item toHaul)
    {
        this.toHaul = toHaul;
        //CreateTask(new HaulTask(this, ));

    }

    public void Query()
    {
    }
}