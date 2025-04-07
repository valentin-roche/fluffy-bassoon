using System;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{   
    private static EventDispatcher instance;
    public static EventDispatcher Instance
    {
        get
        {
            if(instance == null)
            {
                var _instance = new GameObject("EventDispatcher");
                _instance.AddComponent<EventDispatcher>();
            }

            return instance;
        }
    }

    public event Action<int> GetMoneyFromKill;
    public event Action OnCoreDestroyed;
    public event Action<Enemy> OnEnemyDied;

    public Camera MainCamera => mainCamera;
    private Camera mainCamera;

    private void Awake()
    {
        instance = this;
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
