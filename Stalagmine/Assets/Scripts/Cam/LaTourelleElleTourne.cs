using UnityEngine;

public class LaTourelleElleTourne : MonoBehaviour
{
    [SerializeField]
    private Transform transformTurret;
    [SerializeField]
    private float speed;

    void Update()
    {
        transformTurret.Rotate(0,speed * Time.deltaTime, 0);
    }
}
