using System;
using UnityEngine;

public class PouchManager : MonoBehaviour
{
    [SerializeField]
    private int baseValue = 100;

    public event Action<int> PouchValueChanged;

    private int pouchValue;
    public int PouchValue { get => pouchValue; set => SetPouchValue(value); }

    void Start()
    {
        pouchValue = baseValue;
        EventDispatcher.Instance.GetMoneyFromKill += Earn;
    }

    private void SetPouchValue(int value)
    {
        pouchValue = value;
        PouchValueChanged?.Invoke(pouchValue);
    }

    public bool Depense(int value)
    {
        if(value > pouchValue)
        {
            return false;
        }
        PouchValue -= value;
        return true;
    }

    public void Earn(int value) 
    {
        PouchValue += value;
    }
}
