using DG.Tweening;
using System;
using UnityEngine;

public class HUDMenu : MonoBehaviour, ICommunicateWithGameplay
{
    [SerializeField]
    private CanvasGroup fadeInCanvas;

    public event Action SendDataSelection;

    void Start()
    {
        fadeInCanvas.alpha = 1.0f;
        fadeInCanvas.DOFade(0, 0.5f);
    }

    public void GiveContex()
    {

    }
}
