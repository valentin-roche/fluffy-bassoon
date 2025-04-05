namespace Utils
{
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

}