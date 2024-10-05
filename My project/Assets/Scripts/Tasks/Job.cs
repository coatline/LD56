using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    public event System.Action Completed;
    protected List<Task> availableTasks;

    public Job()
    {
    }

    protected virtual void OnCompleted()
    {
        Completed.Invoke();
    }

    public Task TakeAvailableTask()
    {
        if (availableTasks.Count == 0)
            return null;
        availableTasks.RemoveAt(0);
        return availableTasks[0];
    }

    public void TakeTask(Task task) => availableTasks.Remove(task);

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

    public abstract void TaskCompleted();

    public void ReturnTask(Task task)
    {
        availableTasks.Add(task);
    }
}
