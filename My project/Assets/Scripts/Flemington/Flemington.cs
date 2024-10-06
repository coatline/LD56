using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Flemington : MonoBehaviour, IInspectable
{
    public event System.Action<Flemington> Died;

    public Flemington ToTalkTo { get; set; }

    [SerializeField] Stats minStats;
    [SerializeField] Stats maxStats;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] DoingBubble doingBubble;

    public DoingBubble DoingBubble => doingBubble;
    public StateMachine StateMachine { get; private set; }
    public Item Carrying { get; private set; }
    public Stats Stats { get; private set; }
    public House House { get; set; }

    public Vector2 Destination { get; set; }
    public bool Traveling { get; set; }

    public Dictionary<NeedType, NeedBehavior> NeedToBehavior { get; private set; }
    List<Need> needs;
    bool dead;

    void Awake()
    {
        Stats = new Stats(minStats, maxStats);
        StateMachine = new StateMachine(this);

        name = C.GetRandomName();

        NeedToBehavior = new Dictionary<NeedType, NeedBehavior>();
        needs = new List<Need>(DataLibrary.I.Needs.UnsortedArray.ToList());

        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            NeedBehavior behavior = new NeedBehavior(need);
            NeedToBehavior.Add(need.Type, behavior);
            behavior.Died += Die;
        }
    }

    void Update()
    {
        if (dead)
            return;

        if (Carrying != null)
            Carrying.transform.position = Position + new Vector2(0, 0.15f);

        StateMachine.Update(Time.deltaTime);
        UpdateNeeds();
    }

    void UpdateNeeds()
    {
        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            NeedToBehavior[need.Type].Update(Time.deltaTime);
        }
    }

    public NeedBehavior GetBiggestNeed(List<Need> excluding = null)
    {
        NeedBehavior biggestNeed = null;

        for (int i = 0; i < needs.Count; i++)
        {
            NeedBehavior needBehavior = NeedToBehavior[needs[i].Type];

            if (excluding != null && excluding.Contains(needBehavior.Need)) continue;

            if (biggestNeed == null)
                biggestNeed = needBehavior;

            else if (needBehavior.Amount < biggestNeed.Amount)
                biggestNeed = needBehavior;
        }

        return biggestNeed;
    }

    public void PickupItem(Item item)
    {
        Carrying = item;
        Carrying.StartCarrying();
    }

    public void StoreItem(ItemHolder itemHolder)
    {
        itemHolder.SendItem(Carrying);
        Carrying = null;
    }

    void Die(string cause)
    {
        if (dead)
            return;

        dead = true;
        NotificationShower.I.ShowNotification($"{Name} died! Cause: {cause}", 5f);

        // Put our Task back
        StateMachine.Died();

        // Drop what I was carrying
        if (Carrying != null)
        {
            Carrying.StopCarrying();
            Carrying.Reserved = false;
            Carrying = null;
        }

        // TODO: remove from the list of flemingtons

        if (House != null)
        {
            House.Owner = null;
            Village.I.HouseAvailable(House);
        }

        Died?.Invoke(this);
        Destroy(gameObject);
    }

    public bool AtPosition(Vector2 pos, float minDistance) => Mathf.Abs(Position.x - pos.x) < minDistance /*&& Mathf.Abs(Position.y - pos.y) < minDistance * 2*/;

    public Vector2 Position => transform.position;
    public void SetXVelocity(float xVel) => rb.velocity = new Vector2(xVel, rb.velocity.y);
    public void SetYVelocity(float yVel) => rb.velocity = new Vector2(rb.velocity.x, yVel);

    public string Name => name;

    // Summary Tab
    string IInspectable.Content
    {
        get
        {
            string str = "";

            if (House != null)
                str += $"Homeowner\n";
            else
                str += $"Homeless!\n";

            str += $"{StateMachine.GetStateText()}";

            return str;
        }
    }
    string IInspectable.Position => Position.ToString("F1");
    public Transform Transform => transform;
    public event System.Action Destroyed;

    void OnDestroy()
    {
        Destroyed?.Invoke();
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