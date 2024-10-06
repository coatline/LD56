using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public event System.Action<Task> OnCompleted;

    public List<ItemType> NeededItems { get; protected set; }
    public bool IsComplete { get; protected set; }

    public readonly Job RootJob;
    protected readonly int ID;

    public Task(Job rootJob)
    {
        if (rootJob != null)
        {
            RootJob = rootJob;
            RootJob.OnCompleted += ParentJobCompleted;
        }

        ID = GetHashCode();
    }

    public void RemakeAvailable()
    {
        RootJob.ReturnTask(this);
    }

    protected virtual void Completed()
    {
        if (RootJob != null)
            RootJob.OnCompleted -= ParentJobCompleted;

        IsComplete = true;
        OnCompleted?.Invoke(this);
    }

    void ParentJobCompleted(Job job) => Completed();

    public abstract void WorkOn(Flemington flemington, float deltaTime);
    public abstract Vector2 GetTargetPosition();
    public virtual void Cancel() { }

    public virtual string GetTextString() => $"{GetType().Name}\n";
}

public enum TaskType
{
    Break,
    Gather
}