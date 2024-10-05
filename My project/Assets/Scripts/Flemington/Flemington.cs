using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flemington : MonoBehaviour
{
    [SerializeField] Stats minStats;
    [SerializeField] Stats maxStats;

    Stats stats;
    Needs needs;
    Task task;

    void Awake()
    {
        stats = new Stats(minStats, maxStats);
        needs = new Needs();
    }

    void Update()
    {
        needs.Update(Time.deltaTime);

        if (task == null)
            task = Village.I.GetClosestTask(transform.position);

        if (task != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, task.pos, Time.deltaTime * stats.speed);
        }
    }
}

[System.Serializable]
public class Stats
{
    // Vary slightly
    public float speed;
    public float carryingCapacity;

    public Stats(float speed, float carryingCapacity)
    {
        this.speed = speed;
        this.carryingCapacity = carryingCapacity;
    }

    public Stats(Stats minStats, Stats maxStats)
    {
        this.speed = Random.Range(minStats.speed, maxStats.speed);
        this.carryingCapacity = Random.Range(minStats.carryingCapacity, maxStats.carryingCapacity);
    }
}

public class Needs
{
    public float sleep;
    public float food;
    public float drink;
    public float talk;
    public float play;
    public float work;

    public void Update(float dt)
    {
        play -= dt;
        food -= dt;
        drink -= dt;
        talk -= dt;
        play -= dt;
        work -= dt;
    }
}
