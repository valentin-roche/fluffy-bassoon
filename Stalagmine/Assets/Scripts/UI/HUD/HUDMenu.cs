using DG.Tweening;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class HUDMenu : MonoBehaviour, ICommunicateWithGameplay
{
    [SerializeField]
    private CanvasGroup fadeInCanvas;
    [SerializeField] 
    private GameObject turretDisplayPrefab;
    [SerializeField]
    private RectTransform listContainer;
    [SerializeField]
    private List<TurretDefinition> turretsDefinitions;
    [SerializeField]
    private RectTransform turretSelectionPanel;

    private List<GameObject> turretsObjects;

    public event Action SendDataSelection;

    private bool isSelectionShowned = false;

    void Start()
    {
        ToggleShowSelectionPanel(false);
        fadeInCanvas.alpha = 1.0f;
        fadeInCanvas.DOFade(0, 0.5f);
        PopulateTurretList();
    }

    private void OnDestroy()
    {
        if (turretsObjects != null && turretsObjects.Count > 0) 
        {
            foreach (GameObject obj in turretsObjects)
            {
                if(obj.GetComponent<TurretDisplay>() is TurretDisplay display)
                {
                    display.TurretSelected -= OnTurretSelected;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleShowSelectionPanel(!isSelectionShowned);
        }
    }

    public void GiveContex()
    {

    }

    private void PopulateTurretList()
    {
        turretsObjects = new List<GameObject>();

        foreach (TurretDefinition def in turretsDefinitions) 
        {
            GameObject newDisplay = Instantiate(turretDisplayPrefab, listContainer);
            turretsObjects.Add(newDisplay);
            if (newDisplay.GetComponent<TurretDisplay>() is TurretDisplay display)
            {
                display.Display(def);
                display.TurretSelected += OnTurretSelected;
            }
        }
    }

    private void OnTurretSelected(TurretSO turret)
    {

    }

    private void ToggleShowSelectionPanel(bool show)
    {
        isSelectionShowned = show;
        Vector2 destination = show ? new Vector3(turretSelectionPanel.anchoredPosition.x, 0f) : new Vector3(turretSelectionPanel.anchoredPosition.x, -turretSelectionPanel.sizeDelta.y);
        turretSelectionPanel.DOAnchorPos(destination, 0.25f);
    }
}
