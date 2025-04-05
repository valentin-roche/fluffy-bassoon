using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TextMeshProUGUI valueText;

        void Start()
        {
            valueText.text = ((int)slider.value).ToString();

            slider.onValueChanged.AddListener(OnSliderValueChange);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChange);
        }

        private void OnSliderValueChange(float value)
        {
            valueText.text = ((int)value).ToString();
        }
    }
}

