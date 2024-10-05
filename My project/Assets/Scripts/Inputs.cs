using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] CameraFollowWithBarriers camFollow;
    [SerializeField] Inspector inspector;
    [SerializeField] Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject gob = C.GobUnderMouse(C.MouseWorldPosition(cam));

            if (gob != null)
            {
                Hitable b = gob.GetComponent<Hitable>();
                Flemington flem = gob.GetComponent<Flemington>();

                if (b != null)
                    b.Hit();
                else if (flem != null)
                {
                    inspector.Inspect(flem);
                    camFollow.SetFollow(flem.transform);
                }
            }
            else
                inspector.Inspect(null);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Village.I.CreateChunkAt(C.MouseWorldPosition(cam));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Village.I.CreateFlemingtonAt(C.MouseWorldPosition(cam));
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        //Village.I.CreateItemAt(C.MouseWorldPosition(cam));
    }
}