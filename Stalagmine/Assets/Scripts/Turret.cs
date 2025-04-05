using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    public TurretSO TurretSO;

    GameObject currentTarget;

    float timeElapsed = 0;

    List<GameObject> targets;

    private void Start()
    {
        targets = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed > (1.0f/TurretSO.FireRate))
            {
                ShootAtTarget();
                timeElapsed = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            targets.Add(collider.gameObject);
            currentTarget = targets[0];
        }

        Debug.Log("Bah frérot??");
    }

    void ShootAtTarget()
    {
        switch (TurretSO.Projectile)
        {
            case TurretSO.ProjectileType.NORMAL:
                //Call Damage on entity
                break;
            case TurretSO.ProjectileType.FIRE:
                //Call SetFire on entity
                break;
            case TurretSO.ProjectileType.ICE:
                //Call Freeze on Entity
                break;
                default: break;
        }
        Debug.Log("PIOU PIOU");

        //Do cool particle shit
    }
}
