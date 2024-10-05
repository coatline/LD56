using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    public event System.Action<Job> OnCompleted;
    protected readonly List<Task> availableTasks;

    public bool Done { get; private set; }

    public Job()
    {
        availableTasks = new List<Task>();
    }

    protected virtual void Completed()
    {
        OnCompleted.Invoke(this);
    }

    public Task GetAvailableTask()
    {
        if (availableTasks.Count == 0)
            return null;
        return availableTasks[0];
    }

    public void TakeTask(Task task) => availableTasks.Remove(task);

    // Probably won't use this
    public Task GetClosestTask(Vector2 myPos)
    {
        Task closestTask = null;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < availableTasks.Count; i++)
        {
            float dist = Vector2.Distance(availableTasks[i].TargetPosition, myPos);

            if (dist < closestDist)
            {
                closestTask = availableTasks[i];
                closestDist = dist;
            }
        }

        return closestTask;
    }

    public void ReturnTask(Task task)
    {
        availableTasks.Add(task);
    }

    protected virtual void CreateTask(Task task)
    {
        availableTasks.Add(task);
        task.OnCompleted += TaskCompleted;
    }

    protected virtual void TaskCompleted(Task task)
    {
        availableTasks.Remove(task);
    }
}
