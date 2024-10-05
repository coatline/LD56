using UnityEngine;

public abstract class Task
{
    public event System.Action<Task> OnCompleted;

    //public float X { get; protected set; }
    //public float Y { get; protected set; }

    public readonly Job ParentJob;
    protected readonly int ID;

    public Task(Job parentJob)
    {
        ParentJob = parentJob;
        ParentJob.OnCompleted += ParentJobCompleted;
        ID = GetHashCode();
    }

    protected virtual void Completed()
    {
        ParentJob.OnCompleted -= ParentJobCompleted;
        OnCompleted?.Invoke(this);
    }

    void ParentJobCompleted(Job job) => Completed();

    public abstract void WorkOn(Flemington flemington, float deltaTime);
    public abstract Vector2 TargetPosition { get; }

    public virtual string GetTextString() => $"Task: {GetType().Name}\nTask ID: {ID}\n";
}

public enum TaskType
{
    Break,
    Gather
}