using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTask : Task
{
    readonly House house;

    public SleepTask(House house) : base(null, true)
    {
        house.Destroyed += Cancel;
    }

    public override void DoWork(float deltaTime)
    {
        flemington.NeedToBehavior[NeedType.Sleep].ModifyAmount(deltaTime);

        if (flemington.NeedToBehavior[NeedType.Sleep].Amount >= 1)
        {
            SoundManager.I.PlaySound("Flemington Sleep", flemington.transform.position);
            Complete();
        }
    }

    public override void Start(Flemington flemington)
    {
        base.Start(flemington);
        flemington.DoingBubble.ShowSleeping();
    }

    public override void Stop()
    {
        flemington.DoingBubble.StopShowing();
        base.Stop();
    }

    public override void Finish()
    {
        if (house != null)
            house.Destroyed -= Cancel;

        base.Finish();
    }

    public override string GetTextString()
    {
        string str = $"Sleeping";
        return str;
    }

    public override Vector2 GetTargetPosition() => flemington.House.transform.position;
    public override float MinDistance => 0.05f;
}
