using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    readonly Flemington flemington;

    public float IdleStartTime { get; private set; }
    Task idleTask;

    // Ordered in terms of greatest to least needs
    List<Task> needTasks;

    Task currentTask;
    Task rootTask;

    public StateMachine(Flemington flemington)
    {
        this.flemington = flemington;
        idleTask = new IdleTask(flemington);

        SetRootTask(idleTask, idleTask);

        needTasks = new List<Task>();
    }

    public void Update(float deltaTime)
    {
        ChooseNextTask();
        Behave();
    }

    void Behave()
    {
        Vector2 destination = currentTask.GetTargetPosition();

        if (flemington.AtPosition(destination, 0.05f) == false)
        {
            // Update path visual
            flemington.Destination = destination;
            Move(destination);
        }
        else
        {
            // Do work.
            currentTask.DoWork(flemington, Time.deltaTime);
        }
    }

    void RootTaskCanceled(Task task)
    {
        SetRootTask(idleTask, idleTask);
    }

    void CurrentTaskCanceled(Task task)
    {
        SetCurrentTask(idleTask);
    }

    // set root before current
    void SetRootTask(Task newRoot, Task newCurrent)
    {
        // This may be a problem/.....
        if (newRoot == rootTask)
            return;

        if (rootTask != null)
        {
            rootTask.OnCompleted -= RootTaskCanceled;
            rootTask.OnCanceled -= RootTaskCanceled;
        }

        rootTask = newRoot;

        rootTask.OnCompleted += RootTaskCanceled;
        rootTask.OnCanceled += RootTaskCanceled;

        SetCurrentTask(newCurrent);
    }

    void SetCurrentTask(Task newCurrent)
    {
        if (newCurrent == currentTask)
            return;

        if (currentTask != null)
        {
            currentTask.OnCompleted -= CurrentTaskCanceled;
            currentTask.OnCanceled -= CurrentTaskCanceled;
        }

        currentTask = newCurrent;

        if (newCurrent != rootTask)
        {
            currentTask.OnCompleted += CurrentTaskCanceled;
            currentTask.OnCanceled += CurrentTaskCanceled;
        }

        currentTask.Start();
    }

    void Move(Vector2 destination)
    {
        Vector2 dist = (destination - flemington.Position);
        Vector2 movement = dist.normalized * Time.fixedDeltaTime * flemington.Stats.speed * 1f;

        if (Mathf.Abs(flemington.Position.x - destination.x) < 1f)
            flemington.SetYVelocity(movement.y);

        flemington.SetXVelocity(movement.x);
    }

    void ChooseNextTask()
    {
        if (currentTask != idleTask)
            return;

        // Can we do any more work on our current task?
        Task nextTask = rootTask.GetNextTask(flemington);

        // We couldn't do anything more on our root task
        if (nextTask == null)
            SetRootTask(idleTask, idleTask);
        else
        {
            Debug.Log("Doing next task in root..");
            SetCurrentTask(nextTask);
        }

        // If the next thing we can do on our current Root Task is Idle, find a new task.
        if (currentTask == idleTask)
            ChooseNewRootTask();
    }

    void ChooseNewRootTask()
    {
        Debug.Log("Choosing new root..");

        //for (int i = 0; i < needTasks.Count; i++)
        //{
        //    Task needTask = needTasks[i];
        //    Task nextTask = needTask.GetNextTask(flemington);
        //    if (nextTask == null)
        //        continue;
        //    else
        //    {
        //        RootTask = needTask;
        //        CurrentTask = nextTask;
        //    }
        //}

        if (rootTask == idleTask)
            TryGetJobTask();
    }

    void TryGetJobTask()
    {
        // Find the closest doable job.
        List<Task> tried = new();

        do
        {
            // Find the closest Task, excluding those we already thought about
            Task closestRootTask = Village.I.PeekClosestTask(flemington.transform.position, tried);

            // No available or doable tasks
            if (closestRootTask == null)
                break;

            // This is so that we don't get an infinite loop when we keep returning idle, putting the task back on the queue and all that.
            tried.Add(closestRootTask);

            // Evaluate whether or not we can take this task
            Task nextTaskFromThis = closestRootTask.GetNextTask(flemington);

            if (nextTaskFromThis != null)
            {
                // We can do this task.
                closestRootTask.Take();
                SetRootTask(closestRootTask, nextTaskFromThis);
            }
        }
        while (currentTask == idleTask);
    }

    void AddNeedTasks()
    {
        List<Need> checkedNeeds = new List<Need>();
        needTasks = new List<Task>();

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
                        {
                            SleepTask sleepTask = new SleepTask(flemington);
                            needTasks.Add(sleepTask);
                            return;
                        }

                        break;
                    case NeedType.Social:
                        // Is there anyone to talk to?
                        Flemington availableToTalk = Village.I.GetAvailableFlemington(flemington);

                        if (availableToTalk != null)
                        {
                            TalkTask talkTask = new TalkTask(flemington, availableToTalk);
                            needTasks.Add(talkTask);
                        }

                        break;
                    case NeedType.Food:
                        //if (flemington.Carrying != null && flemington.Carrying.Name == "Food")
                        //{

                        //    return new EatState(flemington);
                        //}

                        //Item food = Village.I.GetUnreservedItemOfType(DataLibrary.I.Items["Food"]);

                        //if (food != null)
                        //    return NewTaskState(new GrabTask(food));

                        break;
                }
            }
        }
        while (true);
    }

    public void Died()
    {
        rootTask.DoerJustDied();

        if (rootTask != currentTask)
        {
            currentTask.Cancel();
            //RootTask.Cancel();
        }

        //if (CurrentTask != null)
        //    if (CurrentTask != rootTask)
        //        CurrentTask.Cancel();

        //if (rootTask != null)
        //{
        //    rootTask.RemakeAvailable();
        //    DoneWithRoot();
        //}
    }

    public string GetStateText()
    {
        if (currentTask != null)
        {
            string str = "";

            //if (currentState.RootState != currentState)
            //    str += $"{currentState.RootState.GetInspectText()}\n";

            str += currentTask.GetTextString();

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