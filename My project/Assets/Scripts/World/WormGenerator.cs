using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WormGenerator : MonoBehaviour
{
    [SerializeField] TMP_Text nextWaveText;
    [SerializeField] Worm[] wormPrefabs;
    [SerializeField] Wave[] waves;

    float timer;
    int wave;
    int stage;

    private void Awake()
    {
        timer = waves[0].downTime;
    }

    void Update()
    {
        timer -= TimeManager.I.DeltaTime;

        nextWaveText.text = $"Wave {wave + 1} in {C.DisplayTimeFromSeconds((int)timer)}";

        if (timer <= 0)
        {
            Wave nextWave = CurrentWave();
            SpawnWave(nextWave);
            wave++;
            timer = CurrentWave().downTime;
        }
    }

    void SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.smallWorms; i++)
        {
            Worm worm = Instantiate(wormPrefabs[0], transform);
        }

        for (int i = 0; i < wave.mediumWorms; i++)
        {
            Worm worm = Instantiate(wormPrefabs[1], transform);
        }

        for (int i = 0; i < wave.largeWorms; i++)
        {
            Worm worm = Instantiate(wormPrefabs[2], transform);
        }

        SoundManager.I.PlaySound("Worm Spawn", transform.position);
        timer = wave.downTime;
    }

    Wave CurrentWave() => waves[Mathf.Min(wave, waves.Length - 1)];

    [System.Serializable]
    public class Wave
    {
        public float downTime;
        public int largeWorms;
        public int mediumWorms;
        public int smallWorms;
    }
}
