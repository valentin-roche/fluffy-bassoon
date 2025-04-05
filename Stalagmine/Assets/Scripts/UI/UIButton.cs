using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI Behavior")]
        [SerializeField]           
        public GameObject background;
        [SerializeField]
        private GameObject highlighted;
        [SerializeField]
        private GameObject selected;
        [SerializeField]
        private Button button;

        public Action<UIButton> onClick;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (highlighted != null)
            {
                highlighted.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (highlighted != null)
            {
                highlighted.SetActive(false);
            }
        }

        public void Click()
        {
            onClick?.Invoke(this);
        }

    }
}

