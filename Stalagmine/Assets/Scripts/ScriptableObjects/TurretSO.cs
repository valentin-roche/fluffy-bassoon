using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/Turret", order = 1)]
public class TurretSO : ScriptableObject
{
    public int Range;
    public float FireRate;
    public int Cost;
    public ProjectileType Projectile;
    
    public enum ProjectileType
    {
        NORMAL,
        FIRE,
        ICE
    }
}
