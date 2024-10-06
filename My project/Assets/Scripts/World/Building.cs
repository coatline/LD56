using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : ItemHolder, IInspectable
{
    [SerializeField] ItemStack buildingMaterials;
    [SerializeField] TMP_Text percentText;
    [SerializeField] SpriteRenderer sr;

    float percentComplete;
    protected bool built;

    protected override void Awake()
    {
        base.Awake();
        Village.I.CreateNewJob(new DeliverJob(buildingMaterials, this));
    }

    public override void AddItem(Item item)
    {
        base.AddItem(item);
        percentComplete = PercentComplete();

        if (percentComplete >= 1)
            Completed();
        else
            percentText.text = $"{percentComplete * 100}%";

        sr.color = C.SetAlpha(sr.color, (percentComplete * 0.5f) + 0.5f);
    }

    float PercentComplete()
    {
        int required = 0;
        required = buildingMaterials.Count;
        //for (int i = 0; i < buildingMaterials.Count; i++)
        //{
        //    required += buildingMaterials[i].Count;
        //}
        return (float)GetStoredAmount(buildingMaterials.Item) / required;
    }

    protected virtual void Completed()
    {
        built = true;
        percentText.enabled = false;
    }

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
                str += $"{buildingMaterials.Item.name} ({GetStoredAmount(buildingMaterials.Item)}/{buildingMaterials.Count})";
            }

            return str;
        }
    }

    public event Action Destroyed;
}
