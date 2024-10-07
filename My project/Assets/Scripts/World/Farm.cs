using UnityEngine;
using System.Collections;

public class Farm : Building
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
            yield return new WaitForSeconds(25f);
        }
    }

    void Grow()
    {
        //Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        for (int i = 0; i < spawnPoints.Length; i++)
            Village.I.CreateItemAt(spawnPoints[i].transform.position + new Vector3(0, Random.Range(-0.2f, 0.2f)), DataLibrary.I.Items["Food"]);

        SoundManager.I.PlaySound("Farm Produce", transform.position);
    }

    //public override string Name
    //{
    //    get
    //    {
    //        string str;

    //        if (built)
    //            if (Owner != null)
    //                str = $"{Owner.name}'s House";
    //            else
    //                str = $"Empty House";
    //        else
    //            str = "Unfinished House";

    //        return str;
    //    }
    //}
}