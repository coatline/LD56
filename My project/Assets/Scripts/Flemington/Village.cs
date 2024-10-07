using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Singleton<Village>
{
    public event System.Action<Job> JobCreated;

    [SerializeField] Flemington flemingtonPrefab;
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] Item itemPrefab;

    Dictionary<BuildingType, List<Building>> buildingTypeToBuilding;
    Dictionary<ItemType, List<Item>> typeToItem;
    List<Flemington> flemingtons;
    List<House> availableHomes;
    List<Chunk> chunks;
    List<Item> items;
    List<Job> jobs;

    BuildingType houseType;

    protected override void Awake()
    {
        base.Awake();

        houseType = DataLibrary.I.Buildings["House"];

        buildingTypeToBuilding = new Dictionary<BuildingType, List<Building>>();
        typeToItem = new Dictionary<ItemType, List<Item>>();
        flemingtons = new List<Flemington>();
        availableHomes = new List<House>();
        chunks = new List<Chunk>();
        items = new List<Item>();
        jobs = new List<Job>();
    }

    public void CreateFlemingtonAt(Vector3 pos)
    {
        Flemington newFleminton = Instantiate(flemingtonPrefab, pos, Quaternion.identity);
        newFleminton.Died += FlemingtonDied;
        flemingtons.Add(newFleminton);
        SoundManager.I.PlaySound("Flemington Join", pos);
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

    public Building CreateBuildingAt(Vector3 pos, BuildingType type)
    {
        Building newBuilding = Instantiate(type.Prefab, pos, Quaternion.identity);
        newBuilding.BuildingDestroyed += BuildingDestroyed;
        C.AddToDictionaryWithList(buildingTypeToBuilding, type, newBuilding);
        return newBuilding;
    }

    void BuildingDestroyed(Building building)
    {
        if (building.Type == houseType)
            availableHomes.Remove(building as House);

        C.RemoveFromDictionaryWithList(buildingTypeToBuilding, building.Type, building);
    }

    public void HouseAvailable(House house)
    {
        availableHomes.Add(house);
    }

    public House PeekAvailableHouse()
    {
        if (availableHomes.Count == 0) return null;
        return availableHomes[0];
    }

    public void ClaimHouse(House house, Flemington flemington)
    {
        house.SetOwner(flemington);
        flemington.SetHouse(house);
        availableHomes.Remove(house);
    }

    public Item CreateItemAt(Vector3 pos, ItemType type)
    {
        Item newItem = Instantiate(itemPrefab, pos, Quaternion.identity);
        newItem.Setup(type);
        C.AddToDictionaryWithList<ItemType, Item>(typeToItem, newItem.Type, newItem);
        items.Add(newItem);
        return newItem;
    }

    public void DestroyItem(Item item)
    {
        C.RemoveFromDictionaryWithList(typeToItem, item.Type, item);
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
        job.OnCompleted += RemoveJob;
        job.OnCanceled += RemoveJob;
        jobs.Add(job);
        JobCreated?.Invoke(job);
    }

    void RemoveJob(Job job)
    {
        Debug.Log($"Removing Job {job.GetType().Name}!");
        job.OnCompleted -= RemoveJob;
        job.OnCanceled -= RemoveJob;
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

    //public void TakeTask(Task task)
    //{
    //    task.RootJob.TakeTask(task);
    //}
}