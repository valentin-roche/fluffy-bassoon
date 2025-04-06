using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public AnimationCurve curve;
    bool isRotating = false;

    Quaternion targetvalue;
    private void Start()
    {
        targetvalue = transform.rotation;
    }

    private void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, 90, 0));
            StartCoroutine(PivotCamera());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, -90, 0));
            StartCoroutine(PivotCamera());
        }
    }

    IEnumerator PivotCamera()
    {
        isRotating = true;
        float time = 0;
        Quaternion startValue = transform.rotation;

        while(time < .5f)
        {
            transform.rotation = Quaternion.Lerp(startValue, targetvalue,curve.Evaluate(time/.1f));
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetvalue;
        isRotating = false;
    }
}
