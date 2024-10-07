using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTask : Task
{
    float eatTimer;

    public EatTask() : base(null, true)
    {
        NeededItems = new List<ItemType> { DataLibrary.I.Items["Food"] };
    }

    public override void DoWork(float deltaTime)
    {
        eatTimer += deltaTime;

        if (eatTimer >= 1)
        {
            SoundManager.I.PlaySound("Flemington Eat", flemington.transform.position);

            flemington.NeedToBehavior[NeedType.Food].ModifyAmount(1);
            GameObject.Destroy(flemington.Carrying.gameObject);
            Complete();
        }
    }

    public override void Start(Flemington flemington)
    {
        base.Start(flemington);
        flemington.DoingBubble.ShowEating();
    }

    public override void Stop()
    {
        flemington.DoingBubble.StopShowing();
        base.Stop();
    }

    public override void Finish()
    {
        //flemington.DoingBubble.StopShowing();
        base.Finish();
    }

    public override string GetTextString()
    {
        string str = $"Sleeping";
        return str;
    }

    public override Vector2 GetTargetPosition() => flemington.transform.position;
    public override float MinDistance => 0.05f;
}
