using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsContainer;
        [SerializeField]
        private CanvasGroup fadeOutCanvas;
        [SerializeField]
        private GameObject startMenuContainer;

        [SerializeField]
        private GameObject settingsButtonHighlight;
        [SerializeField]
        private GameObject closeSettingsButtonHighlight;

        private void Start()
        {
            fadeOutCanvas.DOFade(0, 0.5f);
            Display(true);
        }

        public void OnStart()
        {
            Tween fadeOutTween = fadeOutCanvas.DOFade(1, 0.5f);
            fadeOutTween.OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
            fadeOutTween.Play();
        }

        public void OpenSettings()
        {
            Display(false);
            settingsButtonHighlight.SetActive(false);
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
            startMenuContainer.SetActive(display);
            settingsContainer.SetActive(!display);
        }

        public void CloseSettings()
        {
            Display(true);
            closeSettingsButtonHighlight.SetActive(false);
        }
    }
}

