using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private MainSceneRefs mainUI;
    [SerializeField] private TitleSceneRefs titleUI;

    private float enterCreditDelay = 2f;
    private float assignPlayerRefsDelay = 0.1f;

    private int creditCounter = 0;

    private bool isWaiting = false;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private GameEvents gameEvents;
    #endregion

    public TitleSceneRefs TitleUI { get => titleUI; }
    public MainSceneRefs MainUI{ get => mainUI; } 
    public int CreditCounter { get => creditCounter; set => creditCounter = value; }
    public bool IsWaiting => isWaiting;

    private void Awake()
    {
        transform.SetParent(null);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        gameEvents.OnGameOver.AddListener(ShowGameOver);
    }

    private void OnDisable()
    {
        gameEvents.OnGameOver.RemoveListener(ShowGameOver);
    }

    private void Start()
    {
        if (titleUI == null) return;

        titleUI.insertCoinText.enabled = true;
        titleUI.pressStartText.enabled = false;
        titleUI.creditCoinText.text = creditCounter.ToString("00");
    }

    public void EnableGameModes()
    {
        titleUI.modesMenu.SetActive(true);
        titleUI.PVPText.enabled = true;
        titleUI.PVEText.enabled = true;
        titleUI.PVPSelectionArrow.enabled = true;
        titleUI.pressStartText.enabled = false;
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        isWaiting = true;
        titleUI.insertCoinText.enabled = false;
        CreditCounter++;
        titleUI.creditCoinText.text = creditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        titleUI.pressStartText.enabled = true;
        isWaiting = false;
    }

    public void ManageLivesUI(GameObject[] playerLives, int currentLives)
    {
        for (int i = 0; i < playerLives.Length; i++)
        {
            if (i < currentLives)
            {
                playerLives[i].SetActive(true);
            }
            else
            {
                playerLives[i].SetActive(false);
            }
        }
    }

    public void OnPVPSelected()
    {
        titleUI.PVPSelectionArrow.enabled = true;
        titleUI.PVESelectionArrow.enabled = false;
    }

    public void OnPVESelected()
    {
        titleUI.PVPSelectionArrow.enabled = false;
        titleUI.PVESelectionArrow.enabled = true;
    }

    public void TriggerPVPStay()
    {
        titleUI.uiEvents.OnPVPModeStay();
    }

    public void TriggerPVPExit()
    {
        titleUI.uiEvents.OnPVPModeExit();
    }

    public void TriggerPVEStay()
    {
        titleUI.uiEvents.OnPVEModeStay();
    }

    public void TriggerPVEExit()
    {
        titleUI.uiEvents.OnPVEModeExit();
    }

    public void ShowDraw()
    {
        mainUI.drawPanel.SetActive(true);
    }

    public void ShowP1Win()
    {
        mainUI.player1WinsPanel.SetActive(true);
    }

    public void ShowP2Win()
    {
        mainUI.player2WinsPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        mainUI.gameOverPanel.SetActive(true);
    }

    public void EnablePlayers()
    {
        StartCoroutine(EnablePlayersDelay());
    }

    public IEnumerator EnablePlayersDelay()
    {
        yield return new WaitForSeconds(assignPlayerRefsDelay);
        mainUI.player1.SetActive(true);
        mainUI.player2.SetActive(true);
    }

    public void SetMainUI(MainSceneRefs localMainUI)
    {
        mainUI = localMainUI;
    }

    public void SetTitleUI(TitleSceneRefs localTitleUI)
    {
        titleUI = localTitleUI;
    }
}
