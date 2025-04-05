using Unity.VisualScripting;
using UnityEngine;

public class Core : Building
{
    HealthManager HealthManager { get; set; }

    private void Start()
    {
        HealthManager = new(100);
    }

    public void Hit(int damage)
    {
        HealthManager.LoseHealth(damage);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Hit(collider.gameObject.GetComponent<Enemy>().GetSO().Damage);
            collider.gameObject.GetComponent<Enemy>().DestroyEnemy();
        }
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