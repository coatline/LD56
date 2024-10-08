using UnityEngine;
using System.Collections;

public class Farm : ProductionBuilding
{
    protected override void Spawn()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
            Village.I.CreateItemAt(spawnPoints[i].transform.position + new Vector3(0, Random.Range(-0.2f, 0.2f)), DataLibrary.I.Items["Food"]);

        SoundManager.I.PlaySound("Farm Produce", transform.position);
    }

    public override string Content => $"Produces {spawnPoints.Length} food every {spawnInterval}s.\n" + base.Content;
}