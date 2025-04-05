using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    [SerializeField]
    private string sceneNameToLoad;
    [SerializeField]
    private bool loadSceneOnStart = true;

    private ICommunicateWithGameplay currentMenu;

    void Start()
    {
        if(loadSceneOnStart)
        {
            LoadScene();
        }
    }

    private void OnDestroy()
    {
        if(currentMenu != null)
        {
            currentMenu.SendDataSelection += OnDataSelection;
        }
    }

    public async void LoadScene()
    {
        await SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Additive);

        Scene currentUIScene = SceneManager.GetSceneByName(sceneNameToLoad);

        var test = currentUIScene.GetRootGameObjects();

        if(test.Count() > 0)
        {
            foreach(GameObject obj in test)
            {
                if (obj.GetComponent<ICommunicateWithGameplay>() is ICommunicateWithGameplay cummunicate)
                {
                    currentMenu = cummunicate;
                    break;
                }
            }

            if(currentMenu != null)
            {
                currentMenu.GiveContex();
                currentMenu.SendDataSelection += OnDataSelection;
            }
        }
    }

    private void OnDataSelection()
    {

    }
}
