using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class Turret : Building
{
    public TurretSO TurretSO;
    public Light Spotlight;
    public Light Gunlight;

    float shutoffDuration = .5f;

    GameObject currentTarget;

    List<GameObject> targets;

    bool canShoot = true;

    private void Start()
    {
        EventDispatcher.Instance.OnCoreDestroyed += StopShooting;
        EventDispatcher.Instance.OnCoreDestroyed += SpotlightOff;

        targets = new List<GameObject>();
        GetComponent<SphereCollider>().radius = TurretSO.Range;
        Spotlight.spotAngle = (TurretSO.Range*10);
        Spotlight.innerSpotAngle = (TurretSO.Range*5);
        //InvokeRepeating("ShootAction", 0.0f, ((1.0f + Random.Range(-0.05f, 0.05f)) / TurretSO.FireRate));
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.OnCoreDestroyed -= SpotlightOff;
        EventDispatcher.Instance.OnCoreDestroyed -= StopShooting;

    }

    void SpotlightOff()
    {
        StartCoroutine(SpotlightOffCoroutine());
    }

    IEnumerator SpotlightOffCoroutine()
    {
        float time = 0;
        float startValue = Spotlight.intensity;

        while(time < shutoffDuration)
        {
            Spotlight.intensity = Mathf.Lerp(startValue, 0, time / shutoffDuration);
            time += Time.deltaTime;
            yield return null;
        }

        Spotlight.intensity = 0;
    }

    void StopShooting()
    {
        canShoot = false;
    }

    void ShootAction()
    {
        if (currentTarget != null && canShoot)
        {
            ShootAtTarget();
            delayTimer = (1.0f + Random.Range(-0.05f, 0.05f)) / TurretSO.FireRate;
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
            collider.gameObject.GetComponent<Enemy>().OnEnemyAttack += RemoveTarget;

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

    float delayTimer = 0f;

    private void Update()
    {
        delayTimer -= Time.deltaTime;

        if(currentTarget != null && canShoot)
            transform.LookAt(currentTarget.transform);

        if(delayTimer < 0f)
        {
            ShootAction();
        }
    }

    void ShootAtTarget()
    {
        GetComponent<Animator>().SetTrigger("Shoot");
        GetComponent<AudioSource>().Play();
        GetComponentInChildren<ShootLight>().Bang();

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
