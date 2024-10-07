using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image visualsHolder;

    List<Job> jobs;

    private void Awake()
    {
        jobs = new List<Job>();
        Village.I.JobCreated += JobCreated;
    }

    void JobCreated(Job job)
    {
        jobs.Add(job);
        job.OnCompleted += JobCompleted;
        job.OnCanceled += JobCompleted;
        job.NewAvailableTask += UpdateUI;
        UpdateUI();
    }

    void JobCompleted(Job job)
    {
        job.OnCompleted -= JobCompleted;
        job.OnCanceled -= JobCompleted;
        job.NewAvailableTask -= UpdateUI;
        jobs.Remove(job);
        UpdateUI();
    }

    void UpdateUI(Job j = null)
    {
        string str = "";

        for (int i = 0; i < jobs.Count; i++)
        {
            Job job = jobs[i];
            str += $"<color=red>{job.GetType().Name}</color>\n";

            foreach (Task t in job.GetAvailableTasks)
            {
                str += $"<color=green>    {t.GetTextString()}</color>\n";
            }
        }

        text.text = str;
    }

    private void Update()
    {
        UpdateUI();
    }

    public bool IsVisible => visualsHolder.gameObject.activeSelf;
    public void SetVisible(bool visible) => visualsHolder.gameObject.SetActive(visible);
}
