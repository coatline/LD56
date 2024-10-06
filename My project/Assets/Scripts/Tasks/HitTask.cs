using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTask : Task
{
    readonly Hitable toHit;
    float hitTimer;

    public HitTask(Job parentJob, Hitable toHit) : base(parentJob)
    {
        this.toHit = toHit;
        this.toHit.Broken += Completed;
    }

    public override void WorkOn(Flemington flemington, float deltaTime)
    {
        hitTimer += deltaTime;

        if (hitTimer > 1)
            Complete();
    }

    void Complete()
    {
        toHit.Hit();
        Completed();
    }

    protected override void Completed()
    {
        toHit.Broken -= Completed;
        base.Completed();
    }

    public override string GetTextString()
    {
        string str = $"Hitting: {toHit.name} ({toHit.HitPoints}hp)";
        return base.GetTextString() + str;
    }

    public override Vector2 GetTargetPosition() => toHit.transform.position;
}
