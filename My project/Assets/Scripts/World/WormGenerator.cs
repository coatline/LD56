using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGenerator : MonoBehaviour
{
    [SerializeField] float itemSpawnInterval;
    [SerializeField] float initialTimerValue;
    float timer;

    private void Awake()
    {
        timer = initialTimerValue;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= itemSpawnInterval)
        {
            Worm worm = Village.I.CreateWormAt(transform.position);
            timer = 0;
        }
    }
}
