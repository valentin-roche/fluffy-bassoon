using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxValueChanged);
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxValueChanged);
    }

    private void OnMusicValueChanged(float value)
    {

    }

    private void OnSfxValueChanged(float value)
    {

    }
}
