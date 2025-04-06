using System.Collections;
using UnityEngine;

public class ShootLight : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    float startInstensity = 0;
    public float maxIntensity = 10;
    [SerializeField] float duration = .2f;

    private void Start()
    {
        startInstensity = GetComponent<Light>().intensity;
    }

    public void Bang()
    {
        StartCoroutine(BangCoroutine());
    }

    IEnumerator BangCoroutine()
    {
        float time = 0;
        while (time < duration)
        {
            GetComponent<Light>().intensity = Mathf.Lerp(startInstensity, maxIntensity, AnimationCurve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        GetComponent<Light>().intensity = 0;
    }
}
