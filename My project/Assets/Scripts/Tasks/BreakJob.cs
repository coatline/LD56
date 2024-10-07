using UnityEngine;


public class BreakJob : Job
{
    readonly Hitable ToBreak;

    public BreakJob(Hitable toBreak)
    {
        ToBreak = toBreak;
        ToBreak.Broken += Complete;

        for (int i = 0; i < ToBreak.HitPoints; i++)
            CreateTask(NewTask);
    }

    protected override void TaskCompleted(Task task)
    {
        base.TaskCompleted(task);

        if (availableTasks.Count < ToBreak.HitPoints)
            availableTasks.Add(NewTask);
    }

    HitTask NewTask => new HitTask(ToBreak, this);
}