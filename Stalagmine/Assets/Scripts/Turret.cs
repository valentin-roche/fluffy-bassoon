using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class Turret : Building
{
    public TurretSO TurretSO;

    GameObject currentTarget;

    List<GameObject> targets;

    private void Start()
    {


        targets = new List<GameObject>();
        GetComponent<SphereCollider>().radius = TurretSO.Range;
        GetComponentInChildren<Light>().spotAngle = (TurretSO.Range*10);
        GetComponentInChildren<Light>().innerSpotAngle = (TurretSO.Range*5);
        InvokeRepeating("ShootAction", 0.0f, ((1.0f + Random.Range(-0.05f, 0.05f)) / TurretSO.FireRate));
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
            var enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy.IsDyingHelpHim) return;

            targets.Add(collider.gameObject);

            collider.gameObject.GetComponent<Enemy>().OnEnemyDeath += RemoveTarget;

            if (currentTarget == null)
                SetNewTarget();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            RemoveTarget(collider.gameObject);
        }
    }

    void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
        if(target == currentTarget)
        {
            currentTarget = null;
            SetNewTarget();
        }
    }

    void RemoveCurrentTarget()
    {
        if(currentTarget != null)
        {
            targets.Remove(currentTarget);
        }
        SetNewTarget();
    }

    void SetNewTarget()
    {
        if(targets.Count > 0)
        {
            currentTarget = targets[0];

            if(currentTarget == null)
            {
                return;
            }
        }
    }

    private void Update()
    {

        if(currentTarget !=null)
            transform.LookAt(currentTarget.transform);
    }

    void ShootAtTarget()
    {
        GetComponent<Animator>().SetTrigger("Shoot");
        GetComponent<AudioSource>().Play();

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
            case TurretSO.ProjectileType.ZONE:
                break;
                default: break;
        }


        //Do cool particle shit
    }
}
