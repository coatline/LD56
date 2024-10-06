using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    readonly Flemington flemington;

    public Task RootTask { get; private set; }
    public Task CurrentTask
    {
        get
        {
            if (currentTaskState == null) return null;
            return currentTaskState.Task;
        }
    }

    TaskState currentTaskState;
    IdleState idleState;
    State currentState;

    public float idleTime;

    public StateMachine(Flemington flemington)
    {
        this.flemington = flemington;
        idleState = new IdleState(flemington);
        currentState = idleState;
    }

    public void Update(float deltaTime)
    {
        // We finished the last state; choose a new one.
        if (currentState == null || currentState == idleState)
            SetState(ChooseNewState());

        currentState.Update(deltaTime);
    }

    public void SetState(State state)
    {
        if (currentState == state) return;

        // Exit current state
        if (currentState != null)
            currentState.Exit();

        currentState = state;

        // Enter new state
        if (currentState != null)
            currentState.Enter();
    }

    State ChooseNewState()
    {
        // Can we do any more work on our current RootTask?
        State newState = EvaluateTask(RootTask);

        // If we are idle get a job.
        if (newState == idleState)
        {
            List<Need> checkedNeeds = new List<Need>();


            do
            {
                NeedBehavior biggestNeed = flemington.GetBiggestNeed(checkedNeeds);

                if (biggestNeed == null)
                    break;

                checkedNeeds.Add(biggestNeed.Need);

                if (biggestNeed.Severity >= NeedSeverity.Okay)
                {
                    NeedType type = biggestNeed.Need.Type;
                    switch (type)
                    {
                        case NeedType.Sleep:
                            if (flemington.House == null)
                            {
                                House availableHousing = Village.I.PeekAvailableHouse();

                                if (availableHousing != null)
                                    Village.I.ClaimHouse(availableHousing, flemington);
                            }

                            if (flemington.House != null)
                                return new SleepState(flemington);
                            break;
                        case NeedType.Social:
                            // Is there anyone to talk to?
                            Flemington availableToTalk = Village.I.GetAvailableFlemington(flemington);

                            if (availableToTalk != null)
                                return new TalkState(flemington, availableToTalk);
                            break;
                        case NeedType.Food:
                            if (flemington.Carrying != null && flemington.Carrying.Name == "Food")
                            {
                                return new EatState(flemington);
                            }

                            Item food = Village.I.GetUnreservedItemOfType(DataLibrary.I.Items["Food"]);

                            if (food != null)
                                return NewTaskState(new GrabTask(null, food));

                            break;
                    }
                }
            }
            while (true);

            // Get a job.

            List<Task> tried = new();

            idleTime += Time.deltaTime;

            do
            {
                // Find the closest Task, excluding those we already thought about
                Task closestTask = Village.I.PeekClosestTask(flemington.transform.position, tried);

                // No available or doable tasks
                if (closestTask == null)
                    break;

                // This is so that we don't get an infinite loop when we keep returning idle, putting the task back on the queue and all that.
                tried.Add(closestTask);

                // Evaluate whether or not we can take this task
                newState = EvaluateTask(closestTask);

                if (RootTask == closestTask)
                {
                    // We decided we could do this task.
                    Village.I.TakeTask(closestTask);
                    idleTime = 0;
                }
            }
            while (newState == idleState);
        }

        return newState;
    }

    State EvaluateTask(Task task)
    {
        if (task == null || task.IsComplete)
            return idleState;

        // Do we need items?
        if (task.NeededItems != null)
        {
            // Do we have the required items?
            ItemType needed = task.NeededItems[0];

            if (flemington.Carrying != null)
            {
                ItemType itemOwned = flemington.Carrying.Type;

                if (itemOwned != null)
                {
                    // Every requirement has been met, now complete the RootTask!
                    TakeRootTask(task);
                    return NewTaskState(task);
                }
            }
            else
            {
                Item toPickup = Village.I.GetUnreservedItemOfType(needed);

                // Can we find the required items?
                if (toPickup != null)
                {
                    TakeRootTask(task);
                    return NewTaskState(new GrabTask(RootTask.RootJob, toPickup));
                }
                else
                {
                    // We have no way of completing this RootTask.
                    // TODO: mark as insufficient materials.
                    //task.insuffic
                    return idleState;
                }
            }
        }

        // Every requirement has been met, now complete the RootTask!
        TakeRootTask(task);
        return NewTaskState(task);
    }

    TaskState NewTaskState(Task task)
    {
        currentTaskState = new TaskState(task, flemington);
        //currentTaskState.DoneWithTask += DoneWithTask;
        return currentTaskState;
    }

    void TakeRootTask(Task newRoot)
    {
        if (RootTask == newRoot)
            return;

        if (RootTask != null)
            RootTask.OnCompleted -= DoneWithRoot;

        RootTask = newRoot;
        RootTask.OnCompleted += DoneWithRoot;
    }

    public void Died()
    {
        if (CurrentTask != null)
            if (CurrentTask != RootTask)
                CurrentTask.Cancel();

        if (RootTask != null)
        {
            RootTask.RemakeAvailable();
            DoneWithRoot();
        }
    }

    public void DoneWithRoot(Task t = null)
    {
        RootTask.OnCompleted -= DoneWithRoot;
        SetState(idleState);

        currentTaskState.FinishWithTask(null);
        currentTaskState = null;
        RootTask = null;
    }

    public string GetStateText()
    {
        if (currentState != null)
        {
            string str = "";

            if (currentState.RootState != currentState)
                str += $"{currentState.RootState.GetInspectText()}\n";

            str += currentState.GetInspectText();

            return str;
        }
        return "null";
    }

    public enum StateType
    {
        Idle,
        Working,
        Eating,
        Sleeping,
        Playing,
        Talking
    }
}