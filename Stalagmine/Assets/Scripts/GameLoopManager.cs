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
        var toDestroy = LayerParent.transform.GetChild(0);
        var posFin = toDestroy.position;

        for (var i = 0; i < toDestroy.transform.childCount; i++)
        {
            var enf = toDestroy.transform.GetChild(i);
            if (enf != null && enf.GetComponent<MeshRenderer>().isVisible)
                enf.GetComponent<MeshDestroy>().DestroyMesh();
            else if (!enf.GetComponent<MeshRenderer>().isVisible)
                Destroy(enf.gameObject);
        }
        Destroy(toDestroy.gameObject);

        float time = 0;
        float duration = 5f;
        var toGetUp = LayerParent.transform.GetChild(1);
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
