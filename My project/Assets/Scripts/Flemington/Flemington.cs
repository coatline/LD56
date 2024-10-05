using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flemington : MonoBehaviour
{
    [SerializeField] Stats minStats;
    [SerializeField] Stats maxStats;
    [SerializeField] Rigidbody2D rb;

    public Vector3 Destination { get; private set; }
    public bool Traveling { get; private set; }

    Dictionary<Need, float> needToValue;
    StateMachine stateMachine;
    Flemington toTalkTo;
    List<Need> needs;
    Stats stats;

    void Awake()
    {
        stats = new Stats(minStats, maxStats);
        needToValue = new Dictionary<Need, float>();
        needs = new List<Need>(DataLibrary.I.Needs.UnsortedArray.ToList());
        stateMachine = new StateMachine(this);

        name = C.GetRandomName();

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
        if (Traveling == false)
            return;

        Vector2 targetDir = (Destination - transform.position).normalized;

        if (Mathf.Abs(transform.position.x - Destination.x) > 1f)
            targetDir.y = rb.velocity.y;
        else
            targetDir.y *= 3;

        rb.velocity = targetDir * Time.fixedDeltaTime * stats.speed;
        //rb.velocity += new Vector2(0, Mathf.Sin(Time.time));

        if (Vector2.Distance(transform.position, Destination) < 0.05f)
        {
            Traveling = false;
            stateMachine.DestinationReached();
        }
    }

    public void SetDestination(Vector2 pos)
    {
        Destination = pos;
        Traveling = true;
    }

    void UpdateNeeds()
    {
        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            needToValue[need] -= Time.deltaTime / need.TimeToZero;
        }
    }

    public string GetStateText() => stateMachine.GetStateText();
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