using DG.Tweening;
using GameState;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    private Scrollbar scrollbar;

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

    [Header("Game Over Menu")]
    [SerializeField]
    private CanvasGroup gameOverCanvas;

    private List<GameObject> turretsObjects;

    private bool isSelectionShowned = false;

    private PouchManager pouchManager;
    private TerrainManager terrainManager;

    void Start()
    {
        ToggleShowSelectionPanel(false);
        PopulateTurretList();

        ShowTutoPanel(tutoPopupFirst);

        EventDispatcher.Instance.OnCoreDestroyed += OnGameOver;
    }

    private void OnDestroy()
    {
        if(EventDispatcher.Instance)
        {
            EventDispatcher.Instance.OnCoreDestroyed -= OnGameOver;
        }
        
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
        if (Input.GetMouseButtonDown(0) && pauseMenu.activeInHierarchy == false && tutoPopupSecond.gameObject.activeInHierarchy == false && gameOverCanvas.gameObject.activeInHierarchy == false)
        {
            Vector2 mousePos = Input.mousePosition;
            if (isSelectionShowned == false || (isSelectionShowned && turretSelectionPanel.rect.Contains(mousePos) == false))
            {
                if(terrainManager.OnClickLeftMouseToBuild())
                {
                    ToggleShowSelectionPanel(true);
                }                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy)
            {
                Continue();
            }
            else if(isSelectionShowned)
            {
                ToggleShowSelectionPanel(false);
                terrainManager.CancelSelection();
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void GiveContex(PouchManager pouch, TerrainManager terrain)
    {
        pouchManager = pouch;
        pouchContent.text = pouchManager.PouchValue.ToString();
        pouchManager.PouchValueChanged += OnPushValueChanged;
        UpdateTurretList();

        terrainManager = terrain;
    }

    private void PopulateTurretList()
    {
        turretsObjects = new List<GameObject>();

        foreach (TurretDefinition def in turretsDefinitions.OrderBy(x => x.Turret.Cost)) 
        {
            GameObject newDisplay = Instantiate(turretDisplayPrefab, listContainer);
            turretsObjects.Add(newDisplay);
            if (newDisplay.GetComponent<TurretDisplay>() is TurretDisplay display)
            {
                display.Display(def);
                display.TurretSelected += OnTurretSelected;
            }
        }

        scrollbar.value = 0;
    }

    private void OnTurretSelected(TurretSO turret)
    {
        if (pouchManager != null) 
        {
            if(pouchManager.Depense(turret.Cost))
            {
                terrainManager.BuildTurret(turret);
                ToggleShowSelectionPanel(false);
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
            sequence.OnComplete(() => { ShowTutoPanel(tutoPopupSecond); transform.gameObject.SetActive(false); });
        }
        else
        {
            sequence.OnComplete(() => { transform.gameObject.SetActive(false); });
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

    private void OnGameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);
        gameOverCanvas.DOFade(1f, 1f).SetDelay(0.5f);
    }
}
