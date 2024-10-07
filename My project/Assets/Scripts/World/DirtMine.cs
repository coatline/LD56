using UnityEngine;
using System.Collections;

public class DirtMine : Building
{
    [SerializeField] Transform[] spawnPoints;

    protected override void Complete()
    {
        base.Complete();
        StartCoroutine(GrowInterval());
    }

    IEnumerator GrowInterval()
    {
        while (true)
        {
            Grow();
            yield return new WaitForSeconds(8f);
        }
    }

    void Grow()
    {
        //Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        for (int i = 0; i < spawnPoints.Length; i++)
            Village.I.CreateItemAt(spawnPoints[i].transform.position + C.GetRandVector(-0.3f, 0.3f), DataLibrary.I.Items["Dirt"]);

        //SoundManager.I.PlaySound("Farm Produce", transform.position);
    }

    public override string Content => "Produces dirt\n" + base.Content;
}