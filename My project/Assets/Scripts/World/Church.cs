using System.Collections;
using UnityEngine;

public class Church : Building
{
    [SerializeField] Flemington flemingtonPrefab;

    protected override void Complete()
    {
        base.Complete();
        StartCoroutine(GrowInterval());
    }

    IEnumerator GrowInterval()
    {
        while (true)
        {
            SpawnFlem();
            yield return new WaitForSeconds(7f);
        }
    }

    void SpawnFlem()
    {
        Instantiate(flemingtonPrefab, transform.position, Quaternion.identity);
        //SoundManager.I.PlaySound("Farm Produce", transform.position);
    }

    public override string Name => "Church";
    public override string Content => base.Content + "\nCreates new Flemingtons\nProtect at all costs!\n";
}