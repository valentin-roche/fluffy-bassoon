using System;
using UnityEngine;

public class Core : Building
{
    private Utils.HealthManager HealthManager { get; set; }

    public event Action CoreDestroyed;

    private void Start()
    {
        HealthManager = new(100);
    }

    public void Hit(int damage)
    {
        HealthManager.LoseHealth(damage);

        if (HealthManager.IsDead())
        {
            DestroyCore();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Hit(collider.gameObject.GetComponent<Enemy>().GetSO().Damage);
            collider.gameObject.GetComponent<Enemy>().DestroyEnemy();
        }
    }

    void DestroyCore()
    {
        //CoreDestroyed.Invoke();

        GetComponent<AudioSource>().Play();
        Destroy(gameObject, GetComponent<AudioSource>().clip.length+1);
    }

    private void OnDestroy()
    {
        CoreDestroyed?.Invoke();
    }
}
