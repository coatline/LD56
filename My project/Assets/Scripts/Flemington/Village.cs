using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Singleton<Village>
{
    public event System.Action<Job> JobCreated;

    [SerializeField] GameObject gameEndThing;
    [SerializeField] Item itemPrefab;

    Dictionary<BuildingType, List<Building>> buildingTypeToBuilding;
    Dictionary<ItemType, List<Item>> typeToItem;
    List<Building> completedBuildings;
    List<Flemington> flemingtons;
    List<House> availableHomes;
    List<Church> churches;
    List<Item> items;
    List<Job> jobs;

    BuildingType houseType;

    protected override void Awake()
    {
        base.Awake();

        houseType = DataLibrary.I.Buildings["House"];

        buildingTypeToBuilding = new Dictionary<BuildingType, List<Building>>();
        typeToItem = new Dictionary<ItemType, List<Item>>();
        completedBuildings = new List<Building>();
        flemingtons = new List<Flemington>();
        availableHomes = new List<House>();
        churches = new List<Church>();
        items = new List<Item>();
        jobs = new List<Job>();

        Flemington.Created += FlemingtonCreated;
        Building.BuildingCreated += BuildingCreated;
    }

    void FlemingtonCreated(Flemington newFleminton)
    {
        newFleminton.Died += FlemingtonDied;
        flemingtons.Add(newFleminton);
        SoundManager.I.PlaySound("Flemington Join", newFleminton.Position);
        NotificationShower.I.ShowNotification($"{newFleminton.name} joined!", 3f);
    }

    void FlemingtonDied(Flemington dead)
    {
        flemingtons.Remove(dead);
    }

    public Flemington GetAvailableFlemington(Flemington notThisOne)
    {
        if (flemingtons.Count <= 1) return null;

        Flemington other = flemingtons[Random.Range(0, flemingtons.Count)];

        while (other == notThisOne)
            other = flemingtons[Random.Range(0, flemingtons.Count)];

        return other;
    }

    public Building CreateBuildingOfType(BuildingType type, Vector2 pos) => Instantiate(type.Prefab, pos, Quaternion.identity);

    void BuildingCreated(Building newBuilding)
    {
        newBuilding.BuildingDestroyed += BuildingDestroyed;
        newBuilding.BuildingCompleted += BuildingCompleted;

        Church church = newBuilding as Church;

        if (church != null)
            churches.Add(church);

        C.AddToDictionaryWithList(buildingTypeToBuilding, newBuilding.Type, newBuilding);
    }

    public Building GetRandomBuilding()
    {
        if (completedBuildings.Count == 0)
            return null;
        return completedBuildings[Random.Range(0, completedBuildings.Count)];
    }

    void BuildingDestroyed(Building destroyed)
    {
        if (destroyed.Type == houseType)
            availableHomes.Remove(destroyed as House);

        Church church = destroyed as Church;
        if (church)
            churches.Remove(church);

        if (churches.Count == 0)
        {
            if (TimeManager.I.Paused == false)
                TimeManager.I.TogglePaused();

            gameEndThing.SetActive(true);
        }

        completedBuildings.Remove(destroyed);
        C.RemoveFromDictionaryWithList(buildingTypeToBuilding, destroyed.Type, destroyed);
    }

    void BuildingCompleted(Building building)
    {
        completedBuildings.Add(building);

        House house = building as House;

        if (house)
            HouseAvailable(house);
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
        //Debug.Log($"Removing Job {job.GetType().Name}!");
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

    private void OnDestroy()
    {
        Flemington.Created -= FlemingtonCreated;
        Building.BuildingCreated -= BuildingCreated;
    }
}