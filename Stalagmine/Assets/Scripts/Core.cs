using Unity.VisualScripting;
using UnityEngine;

public class Core : Building
{
    HealthManager HealthManager { get; set; }

    public void Hit(int damage)
    {
        HealthManager.LoseHealth(damage);
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