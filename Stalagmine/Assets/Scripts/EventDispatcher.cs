using System;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    public static EventDispatcher Instance;

    public event Action<int> GetMoneyFromKill;

    private void Awake()
    {
        Instance = new EventDispatcher();
    }

    public void EnemyDied(Enemy enemy)
    {
        GetMoneyFromKill?.Invoke(enemy.GetSO().Reward);
    }

}
