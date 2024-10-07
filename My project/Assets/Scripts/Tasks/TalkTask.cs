using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTask : Task
{
    readonly Flemington toTalkTo;
    readonly Flemington flemington;

    public TalkTask(Flemington flemington, Flemington toTalkTo) : base()
    {
        this.flemington = flemington;
        this.toTalkTo = toTalkTo;
        this.toTalkTo.Died += TargetDied;
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        if (flemington.NeedToBehavior[NeedType.Social].Amount < 0.99f)
        {
            flemington.DoingBubble.ShowTalking();
            flemington.NeedToBehavior[NeedType.Social].ModifyAmount(deltaTime);
        }
        else
        {
            Complete();
        }
    }

    void TargetDied(Flemington dead)
    {
        Cancel();
    }

    public override void Cancel()
    {
        flemington.DoingBubble.StopShowing();
        toTalkTo.Died -= TargetDied;
        base.Cancel();
    }

    protected override void Complete()
    {
        flemington.DoingBubble.StopShowing();
        toTalkTo.Died -= TargetDied;
        base.Complete();
    }

    public override string GetTextString()
    {
        string str = $"Chatting with {toTalkTo.Name}";
        return base.GetTextString() + str;
    }

    public override float MinDistance => 0.5f;
    public override Vector2 GetTargetPosition() => toTalkTo.transform.position;
}
