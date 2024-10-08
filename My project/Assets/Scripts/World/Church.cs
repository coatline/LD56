using System.Collections;
using UnityEngine;

public class Church : ProductionBuilding
{
    [SerializeField] Flemington flemingtonPrefab;

    protected override void Spawn()
    {
        Instantiate(flemingtonPrefab, transform.position, Quaternion.identity);
    }

    public override string Name => "Church";
    public override string Content => $"Produces a Flemington every {spawnInterval}s.\n" + base.Content;
}