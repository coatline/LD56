using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTask : Task
{
    public Hitable ToHit { get; private set; }

    public HitTask(Job parentJob, Hitable toHit) : base(parentJob)
    {
        ToHit = toHit;
        ToHit.Broken += Completed;
    }

    public override void WorkOn()
    {
        ToHit.Broken -= Completed;
        ToHit.Hit();
        Completed();
    }

    public override Vector2 TargetPosition => ToHit.transform.position;
}
