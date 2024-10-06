using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inputs : MonoBehaviour
{
    [SerializeField] CameraFollowWithBarriers camFollow;
    [SerializeField] SpriteRenderer mouseSprite;
    [SerializeField] JobDisplayer jobDisplayer;
    [SerializeField] Inspector inspector;
    [SerializeField] Camera cam;

    BuildingType toBuild;

    void Update()
    {
        Vector2 mouseWorldPosition = C.MouseWorldPosition(cam);

        if (Input.GetMouseButtonDown(0))
        {
            if (toBuild != null)
            {
                Village.I.CreateBuildingAt(mouseWorldPosition, toBuild);
            }
            else
            {
                GameObject gob = C.GobUnderMouse(mouseWorldPosition);

                if (gob != null)
                {
                    IInspectable inspectable = gob.GetComponent<IInspectable>();

                    if (inspectable != null)
                    {
                        inspector.Inspect(inspectable);
                        camFollow.SetFollow(inspectable.Transform);
                    }
                }
                else if (C.MouseIsOverUI() == false)
                    inspector.Inspect(null);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (inspector.Inspecting)
                inspector.Inspect(null);
            else
                SetBuildingType(null);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Village.I.CreateChunkAt(mouseWorldPosition);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Village.I.CreateFlemingtonAt(mouseWorldPosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        int key = C.GetNumberKeyDown(1, 3);

        if (key != -1)
            Time.timeScale = key;

        //Village.I.CreateItemAt(C.MouseWorldPosition(cam));

        if (Input.GetKeyDown(KeyCode.F1))
            jobDisplayer.SetVisible(!jobDisplayer.IsVisible);

        mouseSprite.transform.position = mouseWorldPosition;
    }

    public void SetBuildingType(BuildingType type)
    {
        if (inspector.Inspecting)
            inspector.Inspect(null);

        mouseSprite.sprite = type == null ? null : type.Icon;
        toBuild = type;
    }
}