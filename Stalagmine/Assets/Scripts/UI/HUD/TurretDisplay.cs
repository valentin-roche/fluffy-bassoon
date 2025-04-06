using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurretDisplay : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private CanvasGroup canvas;
        [SerializeField]
        private GameObject redFilter;
        [SerializeField]
        private Image turretIcon;
        [SerializeField]
        private TextMeshProUGUI turretName;
        [SerializeField]
        private TextMeshProUGUI attackStat;
        [SerializeField]
        private TextMeshProUGUI fireRateStat;
        [SerializeField]
        private TextMeshProUGUI costStat;
        [SerializeField]
        private TextMeshProUGUI rangeStat;

        private TurretDefinition turretDefinition;
        public TurretDefinition TurretDefinition => turretDefinition;

        public event Action<TurretSO> TurretSelected;

        public void Display(TurretDefinition def)
        {
            turretDefinition = def;

            turretIcon.sprite = turretDefinition.TurretIcon;
            turretName.text = turretDefinition.TurretName;

            TurretSO turret = turretDefinition.Turret;

            attackStat.text = turret.Damage.ToString();
            fireRateStat.text = turret.FireRate.ToString();
            costStat.text = turret.Cost.ToString();
            rangeStat.text = turret.Range.ToString();
        }

        public void Clicked()
        {
            TurretSelected?.Invoke(turretDefinition.Turret);
        }

        public void Disable(bool disable)
        {
            button.interactable = !disable;
            canvas.alpha = disable ? 0.5f : 1f;
            redFilter.SetActive(disable);
        }
    }
}

