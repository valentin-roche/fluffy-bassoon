using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    public TurretSO TurretSO;

    GameObject currentTarget;

    List<GameObject> targets;

    private void Start()
    {
        targets = new List<GameObject>();
        InvokeRepeating("ShootAction", 0.0f, (1.0f / TurretSO.FireRate));
    }


    void ShootAction()
    {
        if (currentTarget != null)
        {
            ShootAtTarget();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy in range " + collider.gameObject.name);

            targets.Add(collider.gameObject);
            if(currentTarget == null)
                SetNewTarget();
        }
    }

    void RemoveCurrentTarget()
    {
        if(currentTarget != null)
        {
            targets.Remove(currentTarget);
        }
    }

    void SetNewTarget()
    {
        Debug.Log("Setting new target");

        if(targets.Count > 0)
        {
            currentTarget = targets[0];
            currentTarget.GetComponent<Enemy>().OnEnemyDeath += RemoveCurrentTarget;
            currentTarget.GetComponent<Enemy>().OnEnemyDeath += SetNewTarget;

            Debug.Log("New target is " + currentTarget.name);
        }
    }

    void ShootAtTarget()
    {
        Debug.Log("PIOU PIOU");
        switch (TurretSO.Projectile)
        {
            case TurretSO.ProjectileType.NORMAL:
                currentTarget.GetComponent<Enemy>().DamageEnemy(TurretSO.Damage);
                break;
            case TurretSO.ProjectileType.FIRE:
                //Call SetFire on entity
                break;
            case TurretSO.ProjectileType.ICE:
                //Call Freeze on Entity
                break;
                default: break;
        }

        //Do cool particle shit
    }
}
