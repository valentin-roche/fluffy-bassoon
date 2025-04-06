using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public AnimationCurve curve;
    [SerializeField] float duration;
    bool isRotating = false;
    bool isMoving = false;

    public Transform[] CameraPositions;
    int positionIndex = 0;

    public Light Spotlight;

    Camera camera;

    Quaternion targetvalue;
    private void Start()
    {
        targetvalue = transform.rotation;
        camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, 90, 0));
                StartCoroutine(PivotCamera());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, -90, 0));
                StartCoroutine(PivotCamera());
            }
        }

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                positionIndex++;
                if(positionIndex >= CameraPositions.Length)
                {
                    positionIndex--;
                    return;
                }

                if(positionIndex > 1)
                {
                    camera.GetComponentInChildren<Light>().enabled = true;
                    Spotlight.enabled = false;
                }
                StartCoroutine(MoveCamera(CameraPositions[positionIndex]));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                positionIndex--;
                if (positionIndex < 0)
                {
                    positionIndex++;
                    return;
                }
                if (positionIndex < 2)
                {
                    camera.GetComponentInChildren<Light>().enabled = false;
                    Spotlight.enabled = true;
                }
                StartCoroutine(MoveCamera(CameraPositions[positionIndex]));
            }
        }
    }

    IEnumerator PivotCamera()
    {
        isRotating = true;
        float time = 0;
        Quaternion startValue = transform.rotation;

        while(time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, targetvalue,curve.Evaluate(time/ duration));
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetvalue;
        isRotating = false;
    }

    IEnumerator MoveCamera(Transform transform)
    {
        isMoving = true;
        float time = 0;
        Quaternion rotStartValue = camera.transform.rotation;
        Vector3 posStartValue = camera.transform.position;


        while (time < duration)
        {
            camera.transform.rotation = Quaternion.Lerp(rotStartValue, transform.rotation, curve.Evaluate(time / duration));
            camera.transform.position = Vector3.Lerp(posStartValue, transform.position, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;
        isMoving = false;
    }
}
