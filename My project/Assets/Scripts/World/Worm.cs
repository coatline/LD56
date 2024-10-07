using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Hitable, IInspectable
{
    [SerializeField] int chunksOnDestroy;
    [SerializeField] int dirtOnDestroy;
    [SerializeField] float moveSpeed;
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] string size;

    Vector3 targetPosition;
    Building targetBuilding;
    float startingY;
    bool havePosition;


    protected override void Awake()
    {
        base.Awake();
        startingY = transform.position.y;
        Village.I.CreateNewJob(new BreakJob(this));
        ChooseTargetBuilding();
    }

    void ChooseTargetBuilding()
    {
        if (targetBuilding != null)
            targetBuilding.Destroyed -= TargetBuildingDestroyed;

        targetBuilding = Village.I.GetRandomBuilding();

        if (targetBuilding != null)
        {
            targetBuilding.Destroyed += TargetBuildingDestroyed;
            targetPosition = targetBuilding.transform.position;
        }
        else if (havePosition == false)
        {
            havePosition = true;
            targetPosition = transform.position + C.GetRandVector(-4f, 4f);
        }
    }

    void TargetBuildingDestroyed()
    {
        ChooseTargetBuilding();
    }

    public override void Hit()
    {
        base.Hit();
        SoundManager.I.PlaySound("Worm Hit", transform.position);
    }

    protected override void Break()
    {
        for (int i = 0; i < chunksOnDestroy; i++)
        {
            Instantiate(chunkPrefab, transform.position + C.GetRandVector(-1f, 1f), Quaternion.identity);
        }

        for (int k = 0; k < dirtOnDestroy; k++)
        {
            Village.I.CreateItemAt(transform.position + C.GetRandVector(-.5f, .5f), DataLibrary.I.Items["Dirt"]);
        }

        SoundManager.I.PlaySound("Worm Die", transform.position);

        base.Break();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, TimeManager.I.DeltaTime * moveSpeed);

        if (targetBuilding == null)
        {
            ChooseTargetBuilding();

            if (targetBuilding == null)
                // TODO: kill nearest Flemington
                return;
        }

        Vector2 direction = (targetPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building)
            Destroy(building.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building)
            Destroy(building.gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (targetBuilding != null)
            targetBuilding.Destroyed -= TargetBuildingDestroyed;

        Destroyed?.Invoke();
    }

    public Transform Transform => transform;
    public string Name => $"{size} Worm";
    public string Position => "";
    public string Content => "Will attack your buildings.";
    public event Action Destroyed;
}
