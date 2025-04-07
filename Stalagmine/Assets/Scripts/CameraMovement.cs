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

    public Light GlobalSpotlight;
    public Light CameraSpotlight;
    public Camera Camera;

    Transform cameraTransform;

    Quaternion targetvalue;
    private void Start()
    {
        targetvalue = transform.rotation;
        cameraTransform = GetComponentInChildren<Camera>().transform.parent;
        EventDispatcher.Instance.SetMainCamera(Camera);
    }

    private void Update()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, 90, 0));
                //Camera.gameObject.GetComponent<CameraShake>().shakeDuration = duration;
                StartCoroutine(PivotCamera());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetvalue = Quaternion.Euler(targetvalue.eulerAngles + new Vector3(0, -90, 0));
                //Camera.gameObject.GetComponent<CameraShake>().shakeDuration = duration;
                StartCoroutine(PivotCamera());
            }
        }

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(positionIndex+1 >= CameraPositions.Length)
                {
                    return;
                }
                positionIndex++;
                Camera.gameObject.GetComponent<CameraShake>().shakeDuration = duration;

                if (positionIndex > 1)
                {
                    CameraSpotlight.enabled = true;
                    GlobalSpotlight.enabled = false;
                }
                StartCoroutine(MoveCamera(CameraPositions[positionIndex]));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (positionIndex-1 < 0)
                {
                    return;
                }
                positionIndex--;
                Camera.gameObject.GetComponent<CameraShake>().shakeDuration = duration;

                if (positionIndex < 2)
                {
                    CameraSpotlight.enabled = false;
                    GlobalSpotlight.enabled = true;
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
        Quaternion rotStartValue = cameraTransform.rotation;
        Vector3 posStartValue = cameraTransform.position;


        while (time < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(rotStartValue, transform.rotation, curve.Evaluate(time / duration));
            cameraTransform.position = Vector3.Lerp(posStartValue, transform.position, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;
        isMoving = false;
    }
}
