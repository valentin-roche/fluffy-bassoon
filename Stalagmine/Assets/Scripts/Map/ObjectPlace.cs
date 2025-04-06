using UnityEngine;

public class ObjectPlace : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown(){
        offset = transform.position - TurretSystem.GetMouseworldPos();
        Debug.Log("ca pose");
    }


}
