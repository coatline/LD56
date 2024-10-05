using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : Hitable
{

    public override void Hit()
    {
        base.Hit();
        Item item = Village.I.CreateItemAt(transform.position);
    }
}
