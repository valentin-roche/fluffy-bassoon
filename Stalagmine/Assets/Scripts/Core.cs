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
    readonly int MaxHealth = 100;
    int Health { get; set; }

    public HealthManager()
    {
        Health = MaxHealth;
    }

    public void LoseHealth(int healthLost)
    {
        Health -= healthLost;
    }

    public void GainHealth(int healthGained)
    {
        Health += healthGained;
    }
}