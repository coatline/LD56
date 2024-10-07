using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public event System.Action<Task> OnCompleted;
    public event System.Action<Task> OnCanceled;
    public event System.Action<Task> DoerDied;
    public event System.Action<Task> Taken;

    public List<ItemType> NeededItems { get; protected set; }
    public bool IsComplete { get; protected set; }

    protected Flemington flemington;
    readonly bool cancelOnDoerDied;
    protected readonly int ID;
    readonly Job rootJob;

    public Task(Job rootJob = null, bool cancelOnDoerDied = false)
    {
        this.rootJob = rootJob;

        if (rootJob != null)
        {
            this.rootJob.OnCanceled += RootJobCanceled;
            this.rootJob.OnCompleted += RootJobComplete;
        }

        this.cancelOnDoerDied = cancelOnDoerDied;
        ID = GetHashCode();
    }

    public void Take()
    {
        Taken?.Invoke(this);
    }

    public virtual void DoerJustDied()
    {
        if (cancelOnDoerDied)
            Cancel();
        else
            Stop();

        DoerDied?.Invoke(this);
    }

    public abstract float MinDistance { get; }
    public abstract void DoWork(float deltaTime);
    public virtual Task GetNextTask(Flemington flemington)
    {
        if (IsComplete)
            return null;

        // Do we need items?
        if (NeededItems != null)
        {
            // Do we have the required items?
            ItemType needed = NeededItems[0];

            if (flemington.Carrying != null)
            {
                ItemType itemOwned = flemington.Carrying.Type;

                if (itemOwned == needed)
                    // Every requirement has been met, now complete the RootTask!
                    return this;
            }
            else
            {
                Item toPickup = Village.I.GetUnreservedItemOfType(needed);

                // Can we find the required items?
                if (toPickup != null)
                    return new GrabTask(toPickup);
                else
                    // We have no way of completing this RootTask.
                    return null;
            }
        }

        return this;
    }

    public virtual void Finish()
    {
        if (rootJob != null)
        {
            this.rootJob.OnCanceled -= RootJobCanceled;
            this.rootJob.OnCompleted -= RootJobComplete;
        }
    }
    public virtual void Start(Flemington flemington) { this.flemington = flemington; }
    public virtual void Stop() { flemington = null; }

    public abstract Vector2 GetTargetPosition();
    void RootJobComplete(Job job) => Cancel();
    void RootJobCanceled(Job job) => Cancel();

    protected virtual void Complete()
    {
        Finish();
        IsComplete = true;
        OnCompleted?.Invoke(this);
    }

    public virtual void Cancel()
    {
        Finish();
        OnCanceled?.Invoke(this);
    }

    public abstract string GetTextString();
}

public enum TaskType
{
    Break,
    Gather
}