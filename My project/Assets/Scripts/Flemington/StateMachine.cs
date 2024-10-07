using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    readonly Flemington flemington;

    public float IdleStartTime { get; private set; }
    Task idleTask;

    Task currentTask;
    Task rootTask;

    public StateMachine(Flemington flemington)
    {
        this.flemington = flemington;
        idleTask = new IdleTask();

        SetRootTask(idleTask, idleTask);
    }

    public void Update(float deltaTime)
    {
        ChooseNextTask();
        Behave(deltaTime);
    }

    void Behave(float deltaTime)
    {
        Vector2 destination = currentTask.GetTargetPosition();

        if (flemington.AtPosition(destination, currentTask.MinDistance) == false)
        {
            // Update path visual
            flemington.Traveling = true;
            flemington.Destination = destination;
            Move(destination, deltaTime);
        }
        else
        {
            // Do work.
            currentTask.DoWork(deltaTime);
            flemington.Traveling = false;
        }
    }

    void RootTaskCanceled(Task task)
    {
        SetRootTask(idleTask, idleTask);
    }

    void CurrentTaskCanceled(Task task)
    {
        Task nextTask = rootTask.GetNextTask(flemington);

        if (nextTask != null)
            SetCurrentTask(nextTask);
        else
            SetRootTask(idleTask, idleTask);
    }

    void SetRootTask(Task newRoot, Task newCurrent)
    {
        if (newRoot == rootTask || flemington.Dead)
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
        if (newCurrent == currentTask || flemington.Dead)
            return;

        if (currentTask != null)
        {
            currentTask.Stop();
            currentTask.OnCompleted -= CurrentTaskCanceled;
            currentTask.OnCanceled -= CurrentTaskCanceled;
        }

        currentTask = newCurrent;

        if (newCurrent != rootTask)
        {
            currentTask.OnCompleted += CurrentTaskCanceled;
            currentTask.OnCanceled += CurrentTaskCanceled;
        }

        currentTask.Start(flemington);
    }

    void Move(Vector2 destination, float deltaTime)
    {
        Vector2 dist = (destination - flemington.Position);
        Vector2 movement = dist.normalized * deltaTime * flemington.Stats.speed;

        if (Mathf.Abs(flemington.Position.x - destination.x) > 1f)
            movement.y = 0;
        flemington.transform.position = Vector3.MoveTowards(flemington.Position, destination, deltaTime);
        //flemington.SetYVelocity(movement.y);

        //flemington.SetXVelocity(movement.x);
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
            SetCurrentTask(nextTask);

        // If the next thing we can do on our current Root Task is Idle, find a new task.
        if (currentTask == idleTask)
            ChooseNewRootTask();
    }

    void ChooseNewRootTask()
    {
        TryGetNeedTask();

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

    void TryGetNeedTask()
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
                        {
                            SleepTask sleepTask = new SleepTask(flemington.House);
                            SetRootTask(sleepTask, sleepTask.GetNextTask(flemington));
                            return;
                        }

                        break;
                    case NeedType.Social:
                        // Is there anyone to talk to?
                        Flemington availableToTalk = Village.I.GetAvailableFlemington(flemington);

                        if (availableToTalk != null)
                        {
                            TalkTask talkTask = new TalkTask(availableToTalk);
                            SetRootTask(talkTask, talkTask.GetNextTask(flemington));
                        }

                        break;
                    case NeedType.Food:

                        EatTask task = new EatTask();
                        Task nextTask = task.GetNextTask(flemington);

                        if (nextTask != null)
                            SetRootTask(task, nextTask);

                        break;
                }
            }
        }
        while (true);
    }

    public void Died()
    {
        rootTask.OnCompleted -= RootTaskCanceled;
        rootTask.OnCanceled -= RootTaskCanceled;
        currentTask.OnCompleted -= CurrentTaskCanceled;
        currentTask.OnCanceled -= CurrentTaskCanceled;

        rootTask.DoerJustDied();

        if (rootTask != currentTask)
            currentTask.DoerJustDied();

        //if (rootTask != currentTask)
        //{
        //    currentTask.Cancel();
        //    RootTask.Cancel();
        //}

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