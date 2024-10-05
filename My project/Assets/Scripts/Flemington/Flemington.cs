using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flemington : MonoBehaviour
{
    [SerializeField] Stats minStats;
    [SerializeField] Stats maxStats;
    [SerializeField] Rigidbody2D rb;

    Vector3 destination;
    bool traveling;

    Dictionary<Need, float> needToValue;
    StateMachine stateMachine;
    Flemington toTalkTo;
    List<Need> needs;
    Stats stats;
    Task task;

    void Awake()
    {
        stats = new Stats(minStats, maxStats);
        needToValue = new Dictionary<Need, float>();
        needs = new List<Need>(DataLibrary.I.Needs.UnsortedArray.ToList());
        stateMachine = new StateMachine(this);

        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            needToValue.Add(need, 1);
        }
    }

    void Update()
    {
        UpdateNeeds();
        stateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (traveling == false)
            return;

        rb.velocity = (destination - transform.position).normalized * Time.fixedDeltaTime * stats.speed;
        rb.velocity += new Vector2(0, Mathf.Sin(Time.time));

        if (Vector2.Distance(transform.position, destination) < 0.25f)
        {
            traveling = false;
            stateMachine.DestinationReached();
        }
    }

    void TryGetTask()
    {
        if (task != null)
            return;

        task = Village.I.GetClosestTask(transform.position);

        if (task != null)
            SetDestination(task.TargetPosition);
    }

    public void SetDestination(Vector2 pos)
    {
        destination = pos;
        traveling = true;
    }

    void UpdateNeeds()
    {
        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            needToValue[need] -= Time.deltaTime / need.TimeToZero;
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