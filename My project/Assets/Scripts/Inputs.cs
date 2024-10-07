using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour
{
    [SerializeField] CameraFollowWithBarriers camFollow;
    [SerializeField] SpriteRenderer mouseSprite;
    [SerializeField] JobDisplayer jobDisplayer;
    [SerializeField] Inspector inspector;
    [SerializeField] Camera cam;

    [SerializeField] Flemington flemingtonPrefab;
    [SerializeField] Chunk chunkPrefab;

    BuildingType toBuild;
    float lastTimeScale;

    void Update()
    {
        Vector2 mouseWorldPosition = C.MouseWorldPosition(cam);

        bool canPlace = true;

        Collider2D[] cols = Physics2D.OverlapBoxAll(mouseWorldPosition, Vector2.one * 0.5f, 0);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].GetComponent<Building>())
                {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace)
            mouseSprite.color = Color.white;
        else
            mouseSprite.color = Color.red;

        if (Input.GetMouseButtonDown(0))
        {
            if (toBuild != null)
            {
                if (canPlace)
                {
                    Village.I.CreateBuildingOfType(toBuild, mouseWorldPosition);
                }
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

        if (Input.GetKey(KeyCode.L))
        {
            GameObject gob = C.GobUnderMouse(mouseWorldPosition);

            if (gob != null && gob.GetComponent<IInspectable>() != null)
                Destroy(gob);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(chunkPrefab, mouseWorldPosition, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(flemingtonPrefab, mouseWorldPosition, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimeManager.I.TogglePaused();
        }

        int key = C.GetNumberKeyDown(1, 5);

        if (key != -1)
            TimeManager.I.SetTimeMultiplier(key);

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

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}