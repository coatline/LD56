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

    protected readonly int ID;

    public Task()
    {
        ID = GetHashCode();
    }

    public void Take()
    {
        Taken?.Invoke(this);
    }

    public void DoerJustDied()
    {
        DoerDied?.Invoke(this);
    }

    protected virtual void Complete()
    {
        IsComplete = true;
        OnCompleted?.Invoke(this);
    }

    public abstract float MinDistance { get; }
    public virtual void Start() { }
    public abstract void DoWork(Flemington flemington, float deltaTime);
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


    public abstract Vector2 GetTargetPosition();
    public virtual void Cancel()
    {
        OnCanceled?.Invoke(this);
    }

    public virtual string GetTextString() => $"{GetType().Name}\n";
}

public enum TaskType
{
    Break,
    Gather
}