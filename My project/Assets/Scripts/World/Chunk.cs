using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : Breakable
{

    public override void Hit()
    {
        base.Hit();
        Item item = Village.I.CreateItemAt(transform.position);
    }
}
