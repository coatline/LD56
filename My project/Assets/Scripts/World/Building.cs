using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Building : ItemHolder, IInspectable
{
    public static Action<Building> BuildingCreated;
    public event Action<Building> BuildingDestroyed;
    public event Action<Building> BuildingCompleted;

    [SerializeField] ItemStack toDeliver;
    [SerializeField] TMP_Text percentText;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] BuildingType type;

    protected ItemStack required;
    float percentComplete;
    protected bool built;

    protected override void Awake()
    {
        base.Awake();

        required = new ItemStack(toDeliver);

        BuildingCreated?.Invoke(this);

        if (required.Count == 0)
            Complete();
        else
            Village.I.CreateNewJob(new DeliverJob(toDeliver, this, 4));
    }

    public override void AddItem(Item item)
    {
        base.AddItem(item);

        percentComplete = PercentComplete();
        SoundManager.I.PlaySound("Item Delivered", transform.position);

        // FIXME: This only allows for one type of material
        toDeliver.Count--;

        if (percentComplete >= 1)
            Complete();
        else
            percentText.text = $"{percentComplete * 100:F0}%";

        sr.color = C.SetAlpha(sr.color, (percentComplete * 0.8f) + 0.2f);
    }

    float PercentComplete()
    {
        int required = 0;
        required = this.required.Count;
        //for (int i = 0; i < buildingMaterials.Count; i++)
        //{
        //    required += buildingMaterials[i].Count;
        //}
        return (float)GetStoredAmount(this.required.Item) / required;
    }

    protected virtual void Complete()
    {
        SoundManager.I.PlaySound("Building Complete", transform.position);
        built = true;
        percentText.enabled = false;
        BuildingCompleted?.Invoke(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BuildingDestroyed?.Invoke(this);
        Destroyed?.Invoke();
    }

    public BuildingType Type => type;

    public event Action Destroyed;
    public Transform Transform => transform;
    public string Position => "";
    public virtual string Name => name;
    public virtual string Content
    {
        get
        {
            string str = "";

            if (built)
                str = "Completed";
            else
            {
                str = $"Required Materials : \n";
                //for (int i = 0; i < buildingMaterials.Count; i++)
                str += $"{required.Item.name} ({GetStoredAmount(required.Item)}/{required.Count})";
            }

            return str;
        }
    }
}
