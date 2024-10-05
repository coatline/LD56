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
    List<Job> jobs;

    protected override void Awake()
    {
        base.Awake();

        flemingtons = new List<Flemington>();
        chunks = new List<Chunk>();
        items = new List<Item>();
        jobs = new List<Job>();
    }

    public void CreateFlemingtonAt(Vector3 pos)
    {
        Flemington newFleminton = Instantiate(flemingtonPrefab, pos, Quaternion.identity);
        flemingtons.Add(newFleminton);
    }

    public Chunk CreateChunkAt(Vector3 pos)
    {
        Chunk newChunk = Instantiate(chunkPrefab, transform.position, Quaternion.identity);
        jobs.Add(new BreakJob(newChunk));
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

        for (int i = 0; i < jobs.Count; i++)
        {
            Job job = jobs[i];
            Task task = job.GetClosestTask(myPos);
            // This can be optimized.
            float dist = Vector2.Distance(myPos, task.TargetPosition);

            if (dist < closestDist)
            {
                closest = task;
                closestDist = dist;
            }
        }

        return closest;
    }
}