using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGenerator : MonoBehaviour
{
    [SerializeField] float itemSpawnInterval;
    [SerializeField] float initialTimerValue;
    [SerializeField] Worm[] wormPrefabs;

    float timer;
    int stage;

    private void Awake()
    {
        timer = initialTimerValue;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= itemSpawnInterval)
        {
            SoundManager.I.PlaySound("Worm Spawn", transform.position);
            Instantiate(wormPrefabs[stage], transform);
            timer = 0;
        }
    }
}
