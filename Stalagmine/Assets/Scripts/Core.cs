using System;
using System.Collections;
using UnityEngine;

public class Core : Building
{
    HealthManager HealthManager { get; set; }

    public event Action CoreDestroyed;
    bool isDestroyed = false;

    [SerializeField] AudioSource CoreDeathAudio;
    [SerializeField] AudioSource CoreHitAudio;

    private void Start()
    {
        HealthManager = new(100);
    }

    public void Hit(int damage)
    {
        HealthManager.LoseHealth(damage);
        CoreHitAudio.Play();

        if (HealthManager.IsDead())
        {
            DestroyCore();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            //Hit(collider.gameObject.GetComponent<Enemy>().GetSO().Damage);
            //collider.gameObject.GetComponent<Enemy>().KillEnemy();
            collider.gameObject.GetComponent<Enemy>().Attack();
        }
    }

    void DestroyCore()
    {
        //CoreDestroyed.Invoke();
        EventDispatcher.Instance.CoreDestroyed();

        if (!isDestroyed)
        {
            isDestroyed = true;
            CoreDeathAudio.Play();
            StartCoroutine(LightOff());
        }

        //Destroy(gameObject, GetComponent<AudioSource>().clip.length+1);
    }

    IEnumerator LightOff()
    {
        float time = 0;
        float startIntensity = GetComponentInChildren<Light>().intensity;

        while(time < 1)
        {
            GetComponentInChildren<Light>().intensity = Mathf.Lerp(startIntensity, 0, time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        GetComponentInChildren<Light>().intensity = 0;
    }

    private void OnDestroy()
    {
        CoreDestroyed?.Invoke();
    }
}

class HealthManager
{
    int MaxHealth { get; }
    public int Health { get; set; }

    public HealthManager(int maxHealth)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
    }

    public void LoseHealth(int healthLost)
    {
        Health -= healthLost;
    }

    public void GainHealth(int healthGained)
    {
        Health += healthGained;
    }

    public bool IsDead() { return Health <= 0; }
}