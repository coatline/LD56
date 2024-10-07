using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTask : Task
{
    readonly Flemington flemington;

    public SleepTask(Flemington flemington) : base()
    {
        this.flemington = flemington;
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        if (flemington.NeedToBehavior[NeedType.Sleep].Amount < 1)
        {
            // Sleep
            flemington.NeedToBehavior[NeedType.Sleep].ModifyAmount(deltaTime);
            flemington.DoingBubble.ShowSleeping();
        }
        else
            Complete();
    }

    protected override void Complete()
    {
        flemington.DoingBubble.StopShowing();
        base.Complete();
    }

    public override Task GetNextTask(Flemington flemington) => this;

    public override string GetTextString()
    {
        string str = $"Sleeping";
        return base.GetTextString() + str;
    }

    public override Vector2 GetTargetPosition() => flemington.House.transform.position;
    public override float MinDistance => 0.05f;
}
