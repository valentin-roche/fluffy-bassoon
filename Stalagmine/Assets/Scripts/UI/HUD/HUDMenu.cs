using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDMenu : MonoBehaviour, ICommunicateWithGameplay
{
    [Header("Turret Selection")]
    [SerializeField] 
    private GameObject turretDisplayPrefab;
    [SerializeField]
    private RectTransform listContainer;
    [SerializeField]
    private List<TurretDefinition> turretsDefinitions;
    [SerializeField]
    private RectTransform turretSelectionPanel;

    [Header("Pause Menu")]
    [SerializeField]
    private GameObject pauseMenu;

    [Header("Tutorials")]
    [SerializeField]
    private RectTransform tutoPopupFirst;
    [SerializeField]
    private RectTransform tutoPopupSecond;

    [Header("Pause Menu")]
    [SerializeField]
    private TextMeshProUGUI pouchContent;

    private List<GameObject> turretsObjects;

    private bool isSelectionShowned = false;

    private PouchManager pouchManager;

    void Start()
    {
        ToggleShowSelectionPanel(false);
        PopulateTurretList();

        ShowTutoPanel(tutoPopupFirst);
    }

    private void OnDestroy()
    {
        if (turretsObjects != null && turretsObjects.Count > 0) 
        {
            foreach (GameObject obj in turretsObjects)
            {
                if(obj != null && obj.GetComponent<TurretDisplay>() is TurretDisplay display)
                {
                    display.TurretSelected -= OnTurretSelected;
                }
            }
        }

        if(pouchManager != null)
        {
            pouchManager.PouchValueChanged -= OnPushValueChanged;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleShowSelectionPanel(!isSelectionShowned);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy)
            {
                Continue();
            }
            else
            {               
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void GiveContex(PouchManager pouch)
    {
        pouchManager = pouch;
        pouchContent.text = pouchManager.PouchValue.ToString();
        pouchManager.PouchValueChanged += OnPushValueChanged;
        UpdateTurretList();
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
        if (pouchManager != null) 
        {
            if(pouchManager.Depense(turret.Cost))
            {
                //place turret
            }
        }
    }

    private void ToggleShowSelectionPanel(bool show)
    {
        isSelectionShowned = show;
        Vector2 destination = show ? new Vector3(turretSelectionPanel.anchoredPosition.x, 0f) : new Vector3(turretSelectionPanel.anchoredPosition.x, -turretSelectionPanel.sizeDelta.y);
        turretSelectionPanel.DOAnchorPos(destination, 0.25f);
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ReturnToStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowTutoPanel(RectTransform transform)
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOAnchorPos(new Vector2(transform.anchoredPosition.x, 0f), 0.5f).SetEase(Ease.OutBack));
        if(transform.GetComponent<CanvasGroup>() is CanvasGroup canvas)
        {
            sequence.Join(canvas.DOFade(1f, 0.25f));
            canvas.blocksRaycasts = true;
        }
        sequence.Play();
    }

    public void HideTutoPanel(RectTransform transform)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOAnchorPos(new Vector2(transform.anchoredPosition.x, -300f), 0.5f).SetEase(Ease.OutQuart));
        if (transform.GetComponent<CanvasGroup>() is CanvasGroup canvas)
        {
            sequence.Join(canvas.DOFade(0f, 0.5f));
            canvas.blocksRaycasts = false;
        }

        if(transform == tutoPopupFirst)
        {
            sequence.OnComplete(() => ShowTutoPanel(tutoPopupSecond));
        }

        sequence.Play();
    }

    private void OnPushValueChanged(int value)
    {
        pouchContent.text = value.ToString();

        UpdateTurretList();
    }

    private void UpdateTurretList()
    {
        if (turretsObjects != null && turretsObjects.Count > 0)
        {
            foreach (GameObject obj in turretsObjects)
            {
                if (obj != null && obj.GetComponent<TurretDisplay>() is TurretDisplay display)
                {
                    display.Disable(display.TurretDefinition.Turret.Cost > pouchManager.PouchValue);
                }
            }
        }
    }
}
