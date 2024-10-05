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
        Chunk newChunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
        CreateNewJob(new BreakJob(newChunk));
        chunks.Add(newChunk);
        return newChunk;
    }

    public Item CreateItemAt(Vector3 pos)
    {
        Item newItem = Instantiate(itemPrefab, pos, Quaternion.identity);
        items.Add(newItem);
        return newItem;
    }

    public void CreateNewJob(Job job)
    {
        job.OnCompleted += JobCompleted;
        jobs.Add(job);
    }

    void JobCompleted(Job job)
    {
        jobs.Remove(job);
    }

    public Task GetClosestTask(Vector3 myPos)
    {
        Task closestTask = null;
        Job closestJob = null;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < jobs.Count; i++)
        {
            Job job = jobs[i];
            Task task = job.GetAvailableTask();
            //Task task = job.GetClosestTask(myPos);

            // All of this job's tasks have been taken
            if (task == null)
                continue;

            // This can be optimized.
            float dist = Vector2.Distance(myPos, task.TargetPosition);

            if (dist < closestDist)
            {
                closestTask = task;
                closestJob = job;
                closestDist = dist;
            }
        }

        if (closestJob != null)
            closestJob.TakeTask(closestTask);
        return closestTask;
    }
}