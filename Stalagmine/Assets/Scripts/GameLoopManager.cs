using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public bool IsPlaying = false;
    public bool IsDead = false;
    public bool IsWin = false;

    public GameObject LayerParent;

    private void Start()
    {
        GetComponent<CoreManager>().Core.CoreDestroyed += GameLost;
    }

    private void OnDestroy()
    {
        GetComponent<CoreManager>().Core.CoreDestroyed -= GameLost;
    }

    void GameLost()
    {
        IsDead = true;
    }

    private void Update()
    {
        if (IsPlaying)
        {
            if(GetComponent<SpawnManager>().SpawnParent.childCount == 0) IsWin = true;
        }
        if(IsWin)
        {
            LayerParent.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (IsDead)
        {

        }
    }
}
