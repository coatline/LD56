using UnityEngine;
using System.Collections;

public class DirtMine : ProductionBuilding
{
    protected override void Spawn()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
            Village.I.CreateItemAt(spawnPoints[i].transform.position + C.GetRandVector(-0.3f, 0.3f), DataLibrary.I.Items["Dirt"]);

        SoundManager.I.PlaySound("Farm Produce", transform.position);
    }

    public override string Content => $"Produces {spawnPoints.Length} dirt every {spawnInterval}s.\n" + base.Content;
}