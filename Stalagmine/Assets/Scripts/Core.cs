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

    [SerializeField] Color CoreHitColor;
    bool CoreDamaged = false;

    private void Start()
    {
        HealthManager = new(100);
    }

    public void Hit(int damage)
    {
        HealthManager.LoseHealth(damage);
        CoreHitAudio.Play();

        if(!CoreDamaged && HealthManager.Health < HealthManager.MaxHealth / 2)
        {
            StartCoroutine(DamagedCore());
        }

        if (HealthManager.IsDead())
        {
            GetComponent<ParticleSystem>().Stop();
            DestroyCore();
        }
    }

    IEnumerator DamagedCore()
    {
        CoreDamaged = true;
        GetComponentInChildren<Light>().color = CoreHitColor;
        GetComponent<ParticleSystem>().Play();
        while (CoreDamaged)
            yield return LightDamaged();
    }

    public AnimationCurve lightCurve;

    IEnumerator LightDamaged()
    {
        float time = 0;
        float maxIntensity = 25;

        while (time < 3)
        {
            GetComponentInChildren<Light>().intensity = Mathf.Lerp(0, maxIntensity,lightCurve.Evaluate(time / 3));
            time += Time.deltaTime;
            yield return null;
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
        CoreDamaged = false;
        //CoreDestroyed.Invoke();
        transform.parent.GetComponentInChildren<MusicManager>().DeathMusic();
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
    public int MaxHealth { get; }
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