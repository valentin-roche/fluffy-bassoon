using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsContainer;
        [SerializeField]
        private GameObject buttonsContainer;
        [SerializeField]
        private GameObject titleContainer;

        private void Start()
        {
            Display(true);
        }

        public void OnStart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenSettings()
        {
            Display(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && settingsContainer.activeInHierarchy)
            {
                Display(true);
            }
        }

        private void Display(bool display)
        {
            titleContainer.SetActive(display);
            buttonsContainer.SetActive(display);
            settingsContainer.SetActive(!display);
        }
    }
}

