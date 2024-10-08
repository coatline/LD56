using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInspectable
{
    [SerializeField] SpriteRenderer sr;
    ItemType type;

    public void Setup(ItemType type)
    {
        this.type = type;
        sr.sprite = type.Sprite;
        name = type.name;
    }

    public bool Reserved { get; set; }
    public ItemType Type => type;

    public string Name => type.name;
    public string Position => transform.position.ToString("F2");
    public string Content => $"Value: {type.Value}\n Reserved: {Reserved}";
    public Transform Transform => transform;
    public event System.Action Destroyed;

    void OnDestroy()
    {
        Destroyed?.Invoke();
    }
}