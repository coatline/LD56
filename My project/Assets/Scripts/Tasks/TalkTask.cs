using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTask : Task
{
    readonly Flemington toTalkTo;
    string subject;

    public TalkTask(Flemington toTalkTo) : base(null, true)
    {
        this.toTalkTo = toTalkTo;
        this.toTalkTo.Died += TargetDied;
        subject = C.GetRandomSubject();
    }

    public override void DoWork(float deltaTime)
    {
        flemington.DoingBubble.ShowTalking();
        flemington.NeedToBehavior[NeedType.Social].ModifyAmount(deltaTime);

        if (flemington.NeedToBehavior[NeedType.Social].Amount >= 1f)
        {
            SoundManager.I.PlaySound("Flemington Talk", flemington.transform.position);
            flemington.conversations.Add($"Talked to {toTalkTo.name} about {subject}.");
            Complete();
        }
    }

    void TargetDied(Flemington dead)
    {
        Cancel();
    }

    public override void Start(Flemington flemington)
    {
        base.Start(flemington);
        flemington.DoingBubble.ShowTalking();
    }

    public override void Stop()
    {
        flemington.DoingBubble.StopShowing();
        toTalkTo.Died -= TargetDied;
        base.Stop();
    }

    public override string GetTextString()
    {
        string str = $"Chatting with {toTalkTo.Name}";
        return str;
    }

    public override float MinDistance => 0.5f;
    public override Vector2 GetTargetPosition() => toTalkTo.transform.position;
}
