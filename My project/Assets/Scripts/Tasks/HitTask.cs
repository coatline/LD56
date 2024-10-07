using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTask : Task
{
    readonly Hitable toHit;
    float hitTimer;

    public HitTask(Hitable toHit) : base()
    {
        this.toHit = toHit;
        this.toHit.Broken += Complete;
    }

    public override void DoWork(Flemington flemington, float deltaTime)
    {
        hitTimer += deltaTime;

        if (hitTimer > 1)
            Complete();
    }

    protected override void Complete()
    {
        toHit.Broken -= Complete;
        toHit.Hit();
        base.Complete();
    }

    public override string GetTextString()
    {
        string str = $"Hitting: {toHit.name} ({toHit.HitPoints}hp)";
        return base.GetTextString() + str;
    }

    public override float MinDistance => 0.2f;
    public override Vector2 GetTargetPosition() => toHit.transform.position;
}
