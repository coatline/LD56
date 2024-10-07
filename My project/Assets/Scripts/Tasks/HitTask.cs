using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTask : Task
{
    readonly Hitable toHit;
    float hitTimer;

    public HitTask(Hitable toHit, Job rootJob) : base(rootJob)
    {
        this.toHit = toHit;
        this.toHit.Broken += Cancel;
    }

    public override void DoWork(float deltaTime)
    {
        hitTimer += deltaTime;

        if (hitTimer > 1)
        {
            toHit.Hit();
            Complete();
        }
    }

    public override void Finish()
    {
        toHit.Broken -= Cancel;
        base.Finish();
    }

    public override string GetTextString()
    {
        string str = $"Hitting {toHit.name} ({toHit.HitPoints}hp)";
        return str;
    }

    public override float MinDistance => 0.2f;
    public override Vector2 GetTargetPosition() => toHit.transform.position;
}
