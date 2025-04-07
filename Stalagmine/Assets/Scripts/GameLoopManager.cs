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

    IEnumerator DestroyLayer()
    {
        var toDestroy = LayerParent.transform.GetChild(1);
        var posFin = toDestroy.position;
        toDestroy.gameObject.SetActive(false);
        //if (toDestroy != null)
        //    toDestroy.GetComponent<MeshDestroy>().DestroyMesh();


        float time = 0;
        float duration = 2f;
        var toGetUp = LayerParent.transform.GetChild(2);
        var posDepart = toGetUp.gameObject.transform.position;
        while (time < duration)
        {
            toGetUp.position = Vector3.Lerp(posDepart, posFin, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        toGetUp.position = posFin;
    }
}
