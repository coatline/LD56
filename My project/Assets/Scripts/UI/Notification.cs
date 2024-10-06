using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] Vector2 padding;
    [SerializeField] RectTransform textRectTransform;
    [SerializeField] TMP_Text text;

    public void DisplayMessage(string message)
    {
        text.text = message;

        background.sizeDelta = new Vector2(text.preferredWidth + padding.x, text.preferredHeight + padding.y);
        //textRectTransform.anchoredPosition = Vector2.zero;
    }

    public RectTransform RectTransform => background;
    public float Height => background.sizeDelta.y;
}
