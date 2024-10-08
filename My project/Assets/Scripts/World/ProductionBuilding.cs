using UnityEngine;
using System.Collections;

public abstract class ProductionBuilding : Building
{
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected float spawnInterval;

    float spawnTimer;

    private void Update()
    {
        if (built)
        {
            if (spawnTimer >= spawnInterval)
            {
                Spawn();
                spawnTimer = 0;
            }
            else
                spawnTimer += TimeManager.I.DeltaTime;
        }
    }

    protected abstract void Spawn();
}