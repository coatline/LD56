using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text inspectText;
    [SerializeField] GameObject tabHolder;
    [SerializeField] Image ui;

    [SerializeField] Transform needBarHolder;
    [SerializeField] NeedBar needBarPrefab;
    Dictionary<NeedType, NeedBar> needToBar;

    IInspectable inspecting;
    Tab currentTab;

    public bool Inspecting => inspecting != null;

    private void Awake()
    {
        needToBar = new Dictionary<NeedType, NeedBar>();

        for (int i = 0; i < DataLibrary.I.Needs.Length; i++)
        {
            NeedBar newNeedBar = Instantiate(needBarPrefab, needBarHolder);
            Need need = DataLibrary.I.Needs[i];
            newNeedBar.Setup(need);
            needToBar.Add(need.Type, newNeedBar);
        }
    }

    public void Inspect(IInspectable inspectable)
    {
        if (inspecting == inspectable)
            return;

        if (inspecting != null)
            inspecting.Destroyed -= InspectingDestroyed;

        inspecting = inspectable;

        if (inspecting == null)
            ui.gameObject.SetActive(false);
        else
        {
            if (inspecting as Flemington)
            {
                tabHolder.SetActive(true);
                SetTab((int)currentTab);
            }
            else
            {
                lineRenderer.enabled = false;
                tabHolder.SetActive(false);
                needBarHolder.gameObject.SetActive(false);
            }

            ui.gameObject.SetActive(true);
            nameText.text = inspecting.Name;
            inspecting.Destroyed += InspectingDestroyed;
        }
    }

    private void Update()
    {
        if (inspecting == null || inspecting.Transform == null)
            return;

        inspectText.text = "";

        Flemington flemington = inspecting as Flemington;

        if (flemington)
        {
            if (flemington.Traveling)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, flemington.transform.position);
                lineRenderer.SetPosition(1, flemington.Destination);
            }
            else
                lineRenderer.enabled = false;

            switch (currentTab)
            {
                case Tab.Summary:
                    inspectText.text += inspecting.Content;
                    break;
                case Tab.Needs:
                    NeedBehavior biggestNeed = flemington.GetBiggestNeed();
                    inspectText.text += $"Needs : {biggestNeed.Need.name} ({biggestNeed.Severity})";

                    for (int i = 0; i < DataLibrary.I.Needs.Length; i++)
                    {
                        Need need = DataLibrary.I.Needs[i];
                        NeedBar bar = needToBar[need.Type];
                        bar.UpdateDisplay(flemington.NeedToBehavior[need.Type].Amount);
                    }
                    break;
                case Tab.Social:
                    break;
                case Tab.Debug:
                    inspectText.text = $"Pos : {inspecting.Position}\n";
                    inspectText.text += inspecting.Content;
                    break;
            }



            //inspectText.text += $"State : {flemington.StateMachine.GetStateText()}\n";
        }
        else
        {
            if (inspecting.Position != "")
                inspectText.text = $"Pos : {inspecting.Position}\n";

            inspectText.text += inspecting.Content;
        }
    }

    void InspectingDestroyed()
    {
        // If we didn't just exit playmode
        if (ui != null)
            Inspect(null);
    }

    public void SetTab(int tab)
    {
        currentTab = (Tab)tab;

        if (currentTab == Tab.Needs)
        {
            needBarHolder.gameObject.SetActive(true);
        }
        else
            needBarHolder.gameObject.SetActive(false);
    }

    enum Tab
    {
        Summary,
        Needs,
        Social,
        Debug
    }
}