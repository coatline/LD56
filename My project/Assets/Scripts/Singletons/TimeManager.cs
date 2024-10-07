using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public event System.Action TimeMultiplierChanged;

    [SerializeField] TMP_Text timeText;

    public float TimeMultiplier { get; private set; }
    /// The in-game time
    public float SimulationTime { get; private set; }
    public float DeltaTime { get; private set; }

    public bool Paused => TimeMultiplier == 0;
    float previousMultiplier;

    protected override void Awake()
    {
        base.Awake();
        SetTimeMultiplier(1);
    }

    public void Update()
    {
        DeltaTime = Time.deltaTime * TimeMultiplier;
        SimulationTime += DeltaTime;
    }

    public void TogglePaused()
    {
        // Toggle pausing

        if (Paused)
            SetTimeMultiplier(previousMultiplier);
        else
        {
            previousMultiplier = TimeMultiplier;
            SetTimeMultiplier(0);
        }
    }

    public void SetTimeMultiplier(float newMultiplier)
    {
        TimeMultiplier = newMultiplier;
        timeText.text = Paused ? "Paused" : $"{newMultiplier}x";
        TimeMultiplierChanged?.Invoke();
    }
}