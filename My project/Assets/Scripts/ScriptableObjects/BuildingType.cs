using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingType : ScriptableObject
{
    [SerializeField] Building buildingPrefab;
    [SerializeField] Sprite icon;

    public Building Prefab => buildingPrefab;
    public Sprite Icon => icon;
}

