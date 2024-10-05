using UnityEngine;

public abstract class Task
{
    public event System.Action OnCompleted;

    public readonly Job ParentJob;

    public Task(Job parentJob)
    {
        ParentJob = parentJob;
        ParentJob.Completed += OnCompleted;
    }

    protected virtual void Completed()
    {
        ParentJob.Completed -= OnCompleted;
        ParentJob.TaskCompleted();
        OnCompleted?.Invoke();
    }

    public abstract void WorkOn();
    public abstract Vector2 TargetPosition { get; }
}

public enum TaskType
{
    Break,
    Gather
}