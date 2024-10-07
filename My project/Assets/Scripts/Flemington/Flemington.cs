using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Flemington : MonoBehaviour, IInspectable
{
    public static event System.Action<Flemington> Created;
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
    public House House { get; private set; }
    public bool Dead { get; private set; }

    public Vector2 Destination { get; set; }
    public bool Traveling { get; set; }

    public Dictionary<NeedType, NeedBehavior> NeedToBehavior { get; private set; }
    public List<string> conversations;
    List<Need> needs;

    void Awake()
    {
        Stats = new Stats(minStats, maxStats);
        StateMachine = new StateMachine(this);

        name = C.GetRandomName();

        NeedToBehavior = new Dictionary<NeedType, NeedBehavior>();
        needs = new List<Need>(DataLibrary.I.Needs.UnsortedArray.ToList());
        conversations = new List<string>();

        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            NeedBehavior behavior = new NeedBehavior(need);
            NeedToBehavior.Add(need.Type, behavior);
            behavior.Died += Die;
        }

        Created?.Invoke(this);
    }

    void Update()
    {
        if (Dead)
            return;

        if (Carrying != null)
            Carrying.transform.position = Position + new Vector2(0, 0.15f);

        UpdateNeeds();
        StateMachine.Update(TimeManager.I.DeltaTime);
    }

    void UpdateNeeds()
    {
        for (int i = 0; i < needs.Count; i++)
        {
            Need need = needs[i];
            NeedToBehavior[need.Type].Update(TimeManager.I.DeltaTime);
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
    }

    public void DropItem()
    {
        Carrying.Reserved = false;
        Carrying = null;
    }

    public virtual void StoreItem(ItemHolder itemHolder)
    {
        itemHolder.SendItem(Carrying);
        Carrying = null;
    }

    public void SetHouse(House house)
    {
        House = house;
        House.Destroyed += MyHouseDestroyed;
    }

    void MyHouseDestroyed()
    {
        House.Destroyed -= MyHouseDestroyed;
        House = null;
    }

    void Die(string cause)
    {
        if (Dead)
            return;

        Dead = true;
        NotificationShower.I.ShowNotification($"<color=red>{Name} died! </color>Cause: {cause}", 5f);
        Destroy(gameObject);
    }

    public bool AtPosition(Vector2 pos, float minDistance) => Mathf.Abs(Position.x - pos.x) < minDistance && Mathf.Abs(Position.y - pos.y) < minDistance;

    public Vector2 Position => transform.position;

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
        // Put our Task back
        StateMachine.Died();

        // Drop what I was carrying
        if (Carrying != null)
            DropItem();

        SoundManager.I.PlaySound("Flemington Die", transform.position);
        Died?.Invoke(this);
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