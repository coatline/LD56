using UnityEngine;


public class BreakJob : Job
{
    Hitable ToBreak { get; private set; }

    public BreakJob(Hitable toBreak)
    {
        ToBreak = toBreak;

        for (int i = 0; i < ToBreak.HitPoints; i++)
            availableTasks.Add(NewTask);
    }

    public override void TaskCompleted()
    {
        if (availableTasks.Count < ToBreak.HitPoints)
            availableTasks.Add(NewTask);
    }

    HitTask NewTask => new HitTask(this, ToBreak);
}