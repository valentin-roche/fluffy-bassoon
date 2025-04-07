using System;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{   
    public static EventDispatcher Instance;

    public event Action<int> GetMoneyFromKill;
    public event Action OnCoreDestroyed;
    public event Action<Enemy> OnEnemyDied;

    public Camera MainCamera => mainCamera;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = new EventDispatcher();
    }

    private void Update()
    {
        if(Instance == null)
        {
            Instance = new EventDispatcher();
        }
    }

    public void EnemyDied(Enemy enemy)
    {
        GetMoneyFromKill?.Invoke(enemy.GetSO().Reward);
        OnEnemyDied?.Invoke(enemy);
    }

    public void CoreDestroyed()
    {
        OnCoreDestroyed?.Invoke();
    }

    public void SetMainCamera(Camera cam)
    {
        mainCamera = cam;
    }

}
