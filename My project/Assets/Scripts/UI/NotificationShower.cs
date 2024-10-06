using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationShower : Singleton<NotificationShower>
{
    [SerializeField] VerticalLayoutGroup vlg;
    [SerializeField] Notification notificationPrefab;
    [SerializeField] float deleteSpeed;
    [SerializeField] float separation;
    [SerializeField] float lerpSpeed;
    List<Notification> noties;

    protected override void Awake()
    {
        base.Awake();
        noties = new List<Notification>();
    }

    public void ShowNotification(string message, float duration)
    {
        Notification newNoti = Instantiate(notificationPrefab, Vector3.zero, Quaternion.identity, transform);
        newNoti.RectTransform.anchoredPosition = new Vector3(0, -TotalSpacing());
        newNoti.DisplayMessage(message);
        noties.Add(newNoti);
        StartCoroutine(Lifetime(newNoti, duration));
        //Destroy(newNoti.gameObject, 2f);
    }

    float TotalSpacing()
    {
        float spacing = 0;
        for (int i = 0; i < noties.Count; i++)
            spacing += noties[i].Height;

        return spacing;
    }

    private void Update()
    {
        for (int i = 0; i < noties.Count; i++)
        {
            Notification noti = noties[i];

            //if (i == 0)
            //    noti.RectTransform.anchoredPosition = Vector2.Lerp(noti.RectTransform.anchoredPosition, Vector2.zero, Time.deltaTime * lerpSpeed);
            if (i != 0)
            {
                Notification olderNoti = noties[i - 1];
                noti.RectTransform.anchoredPosition = olderNoti.RectTransform.anchoredPosition - new Vector2(0, olderNoti.Height);
                //noti.RectTransform.anchoredPosition = Vector2.Lerp(noti.RectTransform.anchoredPosition, olderNoti.RectTransform.anchoredPosition - new Vector2(0, olderNoti.Height), Time.deltaTime * lerpSpeed);
            }
        }
    }

    IEnumerator Lifetime(Notification noti, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        while (noti.RectTransform.sizeDelta.y > 0)
        {
            noti.RectTransform.sizeDelta -= new Vector2(0, Time.deltaTime * deleteSpeed);
            yield return null;
        }

        noties.Remove(noti);
        Destroy(noti.gameObject);
    }
}
