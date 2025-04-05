using UnityEngine;
using System; 
public class ShopManager : MonoBehaviour
{

    [SerializeField]
    public int money {get; private set; }
    [SerializeField]
    private int maxMoney = 999999; 
    [SerializeField]
    private int minMoney = 0; 
    [SerializeField]
    private int startMoney = 100; 

    public static event Action<int> OnMoneyChanged;
    void Start()
    {
        money = startMoney; 
        OnMoneyChanged?.Invoke(money);
    }

    public void AddMoney(int amount)
    {
        if(money + amount < maxMoney){
            money += amount;
        }
        else {
            money = maxMoney; 
        }
        OnMoneyChanged?.Invoke(money); 
         
    }

    public bool SpendMoney(int amount)
    {
        if(money - amount >= minMoney){
            money -= amount; 
            OnMoneyChanged?.Invoke(money);
            return true; 
        }
        else{
             Debug.Log("No Money !");
             return false;
        }
    }

}
