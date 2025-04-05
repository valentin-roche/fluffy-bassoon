using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public enum EnemyType
    {
        Base,
        Sprinter,
        Tank
    }

    public EnemyType enemyType;
    public int Health;
    public float Speed;
    public int Damage;

    public GameObject Prefab;
}
