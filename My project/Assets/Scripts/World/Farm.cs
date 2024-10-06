using UnityEngine;
using System.Collections;

public class Farm : Building
{
    [SerializeField] Transform[] spawnPoints;

    protected override void Completed()
    {
        base.Completed();
        StartCoroutine(GrowInterval());
    }

    IEnumerator GrowInterval()
    {
        while (true)
        {
            Grow();
            yield return new WaitForSeconds(Random.Range(15, 40f));
        }
    }

    void Grow()
    {
        //Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Village.I.CreateFoodAt(spawnPoints[i].transform.position);
        }
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