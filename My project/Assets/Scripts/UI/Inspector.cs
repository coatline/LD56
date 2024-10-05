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
    [SerializeField] Image ui;

    Flemington inspecting;

    public void Inspect(Flemington flemington)
    {
        inspecting = flemington;

        if (inspecting == null)
        {
            //lineRenderer.gameObject.SetActive(false);
            ui.gameObject.SetActive(false);
        }
        else
        {
            //lineRenderer.gameObject.SetActive(true);
            ui.gameObject.SetActive(true);
            nameText.text = inspecting.name;
        }
    }

    private void Update()
    {
        if (inspecting == null)
            return;

        inspectText.text = $"Pos : {inspecting.transform.position.ToString("F1")}\n";

        if (inspecting.Traveling)
        {
            lineRenderer.SetPosition(0, inspecting.transform.position);
            lineRenderer.SetPosition(1, inspecting.Destination);
            inspectText.text += $"Dest : {inspecting.Destination.ToString("F1")}\n";
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.one * 9999);
            lineRenderer.SetPosition(1, Vector3.one * 9999);
        }

        inspectText.text += $"State : {inspecting.GetStateText()}\n";
    }
}