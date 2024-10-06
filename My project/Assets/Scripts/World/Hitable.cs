using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public event System.Action Broken;

    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] protected int startingHP;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeTime;

    private void Awake()
    {
        hp = startingHP;
    }

    float timer;
    int hp;

    public virtual void Hit()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeAnimation());
        hp--;

        if (hp <= 0)
            Break();
    }

    protected virtual void Break()
    {
        Broken?.Invoke();
        Destroy(gameObject);
    }

    IEnumerator ShakeAnimation()
    {
        timer = 0;

        while (timer < shakeTime)
        {
            sr.transform.localPosition = new Vector3(Mathf.Sin(timer), Mathf.Cos(timer)) * shakeCurve.Evaluate(timer / shakeTime);
            timer += Time.deltaTime * shakeSpeed;
            yield return null;
        }

        sr.transform.localPosition = Vector3.zero;
    }

    public int HitPoints => hp;
}
