using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    bool isDyingHelpHim = false;
    public bool IsDyingHelpHim => isDyingHelpHim;

    HealthManager HealthManager { get; set; }
    [SerializeField] private EnemySO EnemySO;

    public event System.Action<GameObject> OnEnemyDeath;

    private Transform target;

    bool gameOver = false;

    public void OnSpawn(EnemySO enemySO, Transform target)
    {
        this.EnemySO = enemySO;
        HealthManager = new(EnemySO.Health);
        this.target = target;

        EventDispatcher.Instance.OnCoreDestroyed += IDLE;
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.OnCoreDestroyed -= IDLE;

    }

    void IDLE()
    {
        gameOver = true;
        GetComponent<Animator>().SetTrigger("GameLost");
    }

    public EnemySO GetSO()
    {
        return EnemySO;
    }

    public void DamageEnemy(int damage)
    {
        GetComponent<AudioSource>().Play();
        GetComponent<ParticleSystem>().Play();
        HealthManager.LoseHealth(damage);

        if (HealthManager.IsDead())
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        isDyingHelpHim = true;

        GetComponent<AudioSource>().Play();

        OnEnemyDeath?.Invoke(this.gameObject);
        EventDispatcher.Instance.EnemyDied(this);

        Destroy(gameObject, GetComponent<AudioSource>().clip.length); // On verra après
    }

    IEnumerator waitCoroutine()
    {
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (target == null || gameOver) return;

        float step = Time.deltaTime * EnemySO.Speed;

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.LookAt(target.position);
    }
}
