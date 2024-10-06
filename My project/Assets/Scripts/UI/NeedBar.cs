using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedBar : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image bar;

    public void Setup(Need need)
    {
        gameObject.SetActive(true);

        gameObject.name = need.name;
        bar.color = need.BarColor;
        nameText.text = need.name;
    }

    public void UpdateDisplay(float amount)
    {
        bar.fillAmount = amount;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
