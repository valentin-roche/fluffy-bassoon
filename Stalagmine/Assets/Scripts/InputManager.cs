using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3? lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public Vector3? GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        else
        {
            lastPosition = null;
        }
        return lastPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponentInParent<WaveManager>().StartWave();
            GetComponentInParent<GameLoopManager>().IsPlaying = true;
        }
    }
}
