using System;
using UnityEngine;

public class NeedBehavior
{
    public event Action<NeedSeverity> NeedSeverityChanged;
    public event Action<string> Died;

    public NeedSeverity Severity { get; private set; }
    public float Amount { get; private set; }
    public readonly Need Need;
    float timeToZero;
    Job getHelpJob;

    public NeedBehavior(Need need)
    {
        Amount = 1;
        Need = need;
        timeToZero = need.TimeToZero;
        Severity = GetSeverity();
    }

    public void Update(float deltaTime)
    {
        if (deltaTime == 0) return;

        ModifyAmount(-deltaTime / timeToZero);
    }

    public void ModifyAmount(float amount)
    {
        Amount = Mathf.Clamp01(Amount + amount);

        NeedSeverity newSeverity = GetSeverity();

        if (Severity != newSeverity)
        {
            NeedSeverityChanged?.Invoke(newSeverity);
            Severity = newSeverity;
        }

        if (Amount <= 0)
        {
            amount = 0;
            Died?.Invoke(Need.name);
        }
    }

    NeedSeverity GetSeverity()
    {
        switch (Amount)
        {
            case < 0.1f: return NeedSeverity.Critical;
            case < 0.25f: return NeedSeverity.Bad;
            case < 0.5f: return NeedSeverity.Okay;
            case < 0.8f: return NeedSeverity.Fine;
            default: return NeedSeverity.Great;
        }
    }
}

public enum NeedSeverity
{
    Great,
    Fine,
    Okay,
    Bad,
    Critical
}