using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float shakeTime;

    float timer;

    public virtual void Hit()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeAnimation());
    }

    IEnumerator ShakeAnimation()
    {
        timer = 0;

        while (timer < shakeTime)
        {
            sr.transform.position = new Vector3(Mathf.Sin(timer), Mathf.Cos(timer)) * shakeCurve.Evaluate(timer / shakeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.transform.position = Vector3.zero;
    }
}
