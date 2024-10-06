using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemType : ScriptableObject
{
    [SerializeField] float value;
    [SerializeField] Sprite sprite;

    public Sprite Sprite => sprite;
    public float Value => value;
}
