using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject gob = C.GobUnderMouse(C.MouseWorldPosition(cam));
            if (gob != null)
            {
                Breakable b = gob.GetComponent<Breakable>();
                print($"{gob.name} {b}");

                if (b != null)
                    b.Hit();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Village.I.CreateFlemingtonAt(C.MouseWorldPosition(cam));
        }
    }
}