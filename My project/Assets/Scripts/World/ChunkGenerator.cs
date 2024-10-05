using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] float itemSpawnInterval;
    float timer;

    void Start()
    {

    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= itemSpawnInterval)
        {
            Village.I.CreateChunkAt(transform.position);
            timer = 0;
        }
    }
}
