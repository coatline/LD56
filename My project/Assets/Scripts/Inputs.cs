using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] CameraFollowWithBarriers camFollow;
    [SerializeField] Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject gob = C.GobUnderMouse(C.MouseWorldPosition(cam));
            if (gob != null)
            {
                Breakable b = gob.GetComponent<Breakable>();
                Flemington flem = gob.GetComponent<Flemington>();
                print($"{gob.name} {b}");

                if (b != null)
                    b.Hit();
                else if (flem != null)
                    camFollow.SetFollow(flem.transform);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Village.I.CreateFlemingtonAt(C.MouseWorldPosition(cam));
        }
    }
}