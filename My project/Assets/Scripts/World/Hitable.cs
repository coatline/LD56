using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public event System.Action Broken;

    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float shakeTime;
    [SerializeField] int hp;

    float timer;

    public virtual void Hit()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeAnimation());
        hp--;

        if (hp <= 0)
        {
            Broken?.Invoke();
            Destroy(gameObject);
        }
    }

    IEnumerator ShakeAnimation()
    {
        timer = 0;

        while (timer < shakeTime)
        {
            sr.transform.localPosition = new Vector3(Mathf.Sin(timer), Mathf.Cos(timer)) * shakeCurve.Evaluate(timer / shakeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.transform.localPosition = Vector3.zero;
    }

    public int HitPoints => hp;
}
