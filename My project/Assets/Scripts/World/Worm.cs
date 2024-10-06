using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Hitable
{
    public override void Hit()
    {
        base.Hit();

        for (int i = 0; i < 2; i++)
        {
            Village.I.CreateItemAt(transform.position, DataLibrary.I.Items["Worm Part"]);
        }
    }

    protected override void Break()
    {
        for (int i = 0; i < 5; i++)
        {
            Chunk chunk = Village.I.CreateChunkAt(transform.position);
            chunk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-15f, 15f), Random.Range(10f, 50f)));
        }

        base.Break();
    }
}
