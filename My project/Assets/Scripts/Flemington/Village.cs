using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Singleton<Village>
{
    [SerializeField] Flemington flemingtonPrefab;
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] Item itemPrefab;

    List<Flemington> flemingtons;
    List<Chunk> chunks;
    List<Item> items;
    List<Task> tasks;

    protected override void Awake()
    {
        base.Awake();

        flemingtons = new List<Flemington>();
        chunks = new List<Chunk>();
        items = new List<Item>();
        tasks = new List<Task>();
    }

    public void CreateFlemingtonAt(Vector3 pos)
    {
        Flemington newFleminton = Instantiate(flemingtonPrefab, pos, Quaternion.identity);
        flemingtons.Add(newFleminton);
    }

    public Chunk CreateChunkAt(Vector3 pos)
    {
        Chunk newChunk = Instantiate(chunkPrefab, transform.position, Quaternion.identity);
        chunks.Add(newChunk);
        return newChunk;
    }

    public Item CreateItemAt(Vector3 pos)
    {
        Item newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        items.Add(newItem);
        return newItem;
    }

    public void AddTask(Task task)
    {
        tasks.Add(task);
    }

    public Task GetClosestTask(Vector3 myPos)
    {
        Task closest = null;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < tasks.Count; i++)
        {
            float dist = Vector2.Distance(tasks[i].pos, myPos);

            if (dist < closestDist)
            {
                closest = tasks[i];
                closestDist = dist;
            }
        }

        return closest;
    }
}

public class Task
{
    public Vector3 pos;
    public ITaskable taskable;

    public Task(Vector3 pos, ITaskable taskable)
    {
        this.pos = pos;
        this.taskable = taskable;
    }
}