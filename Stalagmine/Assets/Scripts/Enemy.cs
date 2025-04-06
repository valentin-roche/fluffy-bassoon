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
    public event System.Action<GameObject> OnEnemyAttack;

    private Transform target;

    bool gameOver = false;

    float randomSpeedMod = 0;

    public void OnSpawn(EnemySO enemySO, Transform target)
    {
        this.EnemySO = enemySO;
        HealthManager = new(EnemySO.Health);
        this.target = target;

        randomSpeedMod = Random.Range(-.01f, .1f);

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
        //GetComponent<AudioSource>().Play();
        GetComponent<ParticleSystem>().Play();
        HealthManager.LoseHealth(damage);

        if (HealthManager.IsDead())
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        Debug.Log("Called");

        isDyingHelpHim = true;

        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("Death");

        OnEnemyDeath?.Invoke(this.gameObject);
        EventDispatcher.Instance.EnemyDied(this);

        //Destroy(gameObject, GetComponent<AudioSource>().clip.length); // On verra après
    }

    public void DestroyEnemy()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(DisappearEnemy());

        //Destroy(gameObject);
    }

    IEnumerator DisappearEnemy()
    {
        float time = 0;
        Vector3 startScale = transform.localScale;

        while(time < 5)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / 5);
            time += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator waitCoroutine()
    {
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    bool isAttacking = false;

    public void Attack()
    {
        OnEnemyAttack?.Invoke(this.gameObject);
        GetComponent<Animator>().SetTrigger("Attack");
        GetComponent<BoxCollider>().enabled = false;
        isAttacking = true;
    }

    public void CoreHit()
    {
        target.gameObject.GetComponent<Core>().Hit(EnemySO.Damage);
    }

    private void FixedUpdate()
    {
        if (target == null || gameOver || isDyingHelpHim || isAttacking) return;

        float step = Time.deltaTime * (EnemySO.Speed + randomSpeedMod);

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.LookAt(target.position);
    }
}
