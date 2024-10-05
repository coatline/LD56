using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
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
            Village.I.CreateItemAt(transform.position);
            timer = 0;
        }
    }
}
