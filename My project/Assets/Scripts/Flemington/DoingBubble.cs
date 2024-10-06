using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoingBubble : MonoBehaviour
{
    [SerializeField] Image bubble;
    [SerializeField] Sprite sleepingSprite;
    [SerializeField] Sprite talkingSprite;
    [SerializeField] Sprite eatingSprite;

    public void ShowTalking()
    {
        bubble.enabled = true;
        bubble.sprite = talkingSprite;
    }

    public void ShowSleeping()
    {
        bubble.enabled = true;
        bubble.sprite = sleepingSprite;
    }

    public void ShowEating()
    {
        bubble.enabled = true;
        bubble.sprite = eatingSprite;
    }

    public void StopShowing()
    {
        bubble.enabled = false;
    }
}
