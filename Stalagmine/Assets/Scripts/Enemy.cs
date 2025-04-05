using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    HealthManager HealthManager { get; set; }
    public EnemySO EnemySO;

    public event System.Action OnEnemyDeath;

    private void Start()
    {
        HealthManager = new(EnemySO.Health);
    }

    public void DamageEnemy(int damage)
    {
        GetComponent<AudioSource>().Play();
        HealthManager.LoseHealth(damage);

        if (HealthManager.IsDead())
        {
            OnEnemyDeath.Invoke();
            Destroy(gameObject, GetComponent<AudioSource>().clip.length); // On verra après
        }
    }

    IEnumerator waitCoroutine()
    {
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

}
