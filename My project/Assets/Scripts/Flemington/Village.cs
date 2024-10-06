using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Singleton<Village>
{
    public event System.Action<Job> JobCreated;

    [SerializeField] Flemington flemingtonPrefab;
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] Worm wormPrefab;
    [SerializeField] Item itemPrefab;

    Dictionary<ItemType, List<Item>> typeToItem;
    List<Flemington> flemingtons;
    List<House> availableHouses;
    List<Chunk> chunks;
    List<Item> items;
    List<Item> food;
    List<Job> jobs;

    protected override void Awake()
    {
        base.Awake();

        typeToItem = new Dictionary<ItemType, List<Item>>();
        flemingtons = new List<Flemington>();
        availableHouses = new List<House>();
        chunks = new List<Chunk>();
        items = new List<Item>();
        food = new List<Item>();
        jobs = new List<Job>();
    }

    public void CreateFlemingtonAt(Vector3 pos)
    {
        Flemington newFleminton = Instantiate(flemingtonPrefab, pos, Quaternion.identity);
        newFleminton.Died += FlemingtonDied;
        flemingtons.Add(newFleminton);
        NotificationShower.I.ShowNotification($"{newFleminton.name} joined!", 3f);
    }

    void FlemingtonDied(Flemington dead)
    {
        flemingtons.Remove(dead);
    }

    public Flemington GetAvailableFlemington(Flemington notThisOne)
    {
        if (flemingtons.Count == 0) return null;

        if (flemingtons.Count == 1)
            return null;

        if (flemingtons[0] == notThisOne)
            return flemingtons[1];

        return flemingtons[0];
    }

    //public Flemington GetAvailableFlemington

    public Chunk CreateChunkAt(Vector3 pos)
    {
        Chunk newChunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
        CreateNewJob(new BreakJob(newChunk));
        chunks.Add(newChunk);
        return newChunk;
    }

    public Worm CreateWormAt(Vector3 pos)
    {
        Worm newWorm = Instantiate(wormPrefab, pos, Quaternion.identity);
        CreateNewJob(new BreakJob(newWorm));
        return newWorm;
    }

    public Building CreateBuildingAt(Vector3 pos, BuildingType type)
    {
        Building newBuilding = Instantiate(type.Prefab, pos, Quaternion.identity);
        return newBuilding;
    }

    public void HouseAvailable(House house)
    {
        availableHouses.Add(house);
    }

    public House PeekAvailableHouse()
    {
        if (availableHouses.Count == 0) return null;
        return availableHouses[0];
    }

    public void ClaimHouse(House house, Flemington flemington)
    {
        house.Owner = flemington;
        flemington.House = house;
        availableHouses.Remove(house);
    }

    public Item CreateItemAt(Vector3 pos, ItemType type)
    {
        Item newItem = Instantiate(itemPrefab, pos, Quaternion.identity);
        newItem.Setup(type);
        C.AddToDictionaryWithList<ItemType, Item>(typeToItem, newItem.Type, newItem);
        items.Add(newItem);
        return newItem;
    }

    public Item CreateFoodAt(Vector3 pos)
    {
        Item newItem = CreateItemAt(pos, DataLibrary.I.Items["Food"]);
        food.Add(newItem);
        return newItem;
    }

    public void DestroyItem(Item item)
    {
        C.RemoveFromDictionaryWithList(typeToItem, item.Type, item);

        if (item.Name == "Food")
            food.Remove(item);

        Destroy(item.gameObject);
    }

    public Item GetUnreservedItemOfType(ItemType type)
    {
        if (typeToItem.TryGetValue(type, out List<Item> items))
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                if (item.Reserved == false)
                    return item;
            }
        }

        return null;
    }

    public void CreateNewJob(Job job)
    {
        job.OnCompleted += JobCompleted;
        jobs.Add(job);
        JobCreated?.Invoke(job);
    }

    void JobCompleted(Job job)
    {
        jobs.Remove(job);
    }

    public Task PeekClosestTask(Vector3 myPos, List<Task> exclude)
    {
        Task closestTask = null;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < jobs.Count; i++)
        {
            Task task = jobs[i].PeekAvailableTask();
            //Task task = job.GetClosestTask(myPos);

            // All of this job's tasks have been taken
            if (task == null || exclude.Contains(task))
                continue;

            // This can be optimized.
            float dist = Vector2.Distance(myPos, task.GetTargetPosition());

            if (dist < closestDist)
            {
                closestTask = task;
                closestDist = dist;
            }
        }

        return closestTask;
    }

    public void TakeTask(Task task)
    {
        task.RootJob.TakeTask(task);
    }
}