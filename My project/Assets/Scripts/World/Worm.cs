using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Hitable
{
    [SerializeField] int itemsPerHit;
    [SerializeField] int chunksOnDestroy;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveOutOfGroundAmount;

    bool outOfGround;
    float startingY;

    protected override void Awake()
    {
        base.Awake();
        startingY = transform.position.y;
        Village.I.CreateNewJob(new BreakJob(this));
    }

    public override void Hit()
    {
        base.Hit();

        for (int i = 0; i < itemsPerHit; i++)
        {
            Village.I.CreateItemAt(transform.position, DataLibrary.I.Items["Worm Part"]);
        }

        SoundManager.I.PlaySound("Worm Hit", transform.position);
    }

    protected override void Break()
    {
        for (int i = 0; i < chunksOnDestroy; i++)
        {
            Chunk chunk = Village.I.CreateChunkAt(transform.position);
            chunk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-15f, 15f), Random.Range(10f, 50f)));
        }

        SoundManager.I.PlaySound("Worm Die", transform.position);

        base.Break();
    }

    private void Update()
    {
        if (outOfGround == false)
        {
            if (Mathf.Abs(startingY - transform.position.y) >= moveOutOfGroundAmount)
                outOfGround = true;
            else
                transform.Translate(new Vector3(0, Time.deltaTime * moveSpeed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building)
        {
            Destroy(building.gameObject);
        }
    }
}
