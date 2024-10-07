using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : Hitable, IInspectable
{
    public override void Hit()
    {
        base.Hit();
        Item item = Village.I.CreateItemAt(transform.position, DataLibrary.I.Items["Worm Part"]);
        SoundManager.I.PlaySound("Worm Hit", transform.position);
    }

    protected override void Break()
    {
        for (int i = 0; i < 2; i++)
        {
            Item item = Village.I.CreateItemAt(transform.position, DataLibrary.I.Items["Worm Part"]);
        }
        for (int i = 0; i < 2; i++)
        {
            Item item = Village.I.CreateItemAt(transform.position, DataLibrary.I.Items["Food"]);
        }

        SoundManager.I.PlaySound("Worm Hit", transform.position);
        SoundManager.I.PlaySound("Worm Hit", transform.position);
        base.Break();
    }

    public Transform Transform => transform;
    public string Name => name;
    public string Position => "";
    public string Content => $"{HitPoints}/{startingHP}";
    public event System.Action Destroyed;

    void OnDestroy()
    {
        Destroyed?.Invoke();
    }
}
