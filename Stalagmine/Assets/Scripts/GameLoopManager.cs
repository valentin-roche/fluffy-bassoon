using System.Collections;
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
            if (GetComponent<SpawnManager>().SpawnParent.childCount == 0)
            {
                IsPlaying = false;
                IsWin = true;
            }
        }
        if(IsWin)
        {
            IsWin = false;
            StartCoroutine(DestroyLayer());
        }
        if (IsDead)
        {

        }
    }

    public IEnumerator DestroyLayer()
    {
        LayerParent.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

        for (var i = 1; i < LayerParent.transform.GetChild(0).transform.childCount; i++)
        {
            var enf = LayerParent.transform.GetChild(0).transform.GetChild(i);
            if (enf != null)
                enf.GetComponent<MeshDestroy>().DestroyMesh();
        }
        yield return null;
    }
}
