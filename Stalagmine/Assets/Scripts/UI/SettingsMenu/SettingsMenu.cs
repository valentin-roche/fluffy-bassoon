using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioMixer audioMixer;

    void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxValueChanged);

        float musicMixerValue;
        float sfxMixerValue;
        audioMixer.GetFloat("MusicVolume", out musicMixerValue);
        musicSlider.value = Mathf.Pow(10, (musicMixerValue / 20f));
        audioMixer.GetFloat("SfxVolume", out sfxMixerValue);
        sfxSlider.value = Mathf.Pow(10, (sfxMixerValue / 20f));
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxValueChanged);
    }

    private void OnMusicValueChanged(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value)*20);
    }

    private void OnSfxValueChanged(float value)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }
}
