using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private MainSceneRefs mainSceneRefs;
    [SerializeField] private TitleSceneRefs titleSceneRefs;

    private float enterCreditDelay = 2f;
    private int creditCounter = 0;
    private bool isWaiting = false;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private GameEvents gameEvents;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject modesMenu;
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI pvpText;
    [SerializeField] private TextMeshProUGUI pveText;
    [SerializeField] private TextMeshProUGUI pvpSelectionArrow;
    [SerializeField] private TextMeshProUGUI pveSelectionArrow;
    #endregion

    public TitleSceneRefs TitleSceneRefs { get => titleSceneRefs; }
    public MainSceneRefs MainSceneUIRefs { get => mainSceneRefs; } 
    public TextMeshProUGUI PVPText { get => pvpText; }
    public TextMeshProUGUI PVEText { get => pveText; }
    public TextMeshProUGUI PVPSelectionArrow { get => pvpSelectionArrow; }
    public TextMeshProUGUI PVESelectionArrow { get => pveSelectionArrow; }
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
        titleSceneRefs.insertCoinText.enabled = true;
        titleSceneRefs.pressStartText.enabled = false;
        titleSceneRefs.creditCoinText.text = creditCounter.ToString("00");
    }

    public void EnableGameModes()
    {
        modesMenu.SetActive(true);
        PVPText.enabled = true;
        PVEText.enabled = true;
        PVPSelectionArrow.enabled = true;
        titleSceneRefs.pressStartText.enabled = false;
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        isWaiting = true;
        titleSceneRefs.insertCoinText.enabled = false;
        CreditCounter++;
        titleSceneRefs.creditCoinText.text = creditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        titleSceneRefs.pressStartText.enabled = true;
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

    public void ShowGameOver()
    {
        mainSceneRefs.gameOverPanel.SetActive(true);
    }

    public void AssignMainSceneRefsAtRuntime(MainSceneRefs localMainSceneUIRefs)
    {
        mainSceneRefs = localMainSceneUIRefs;
    }

    public void AssignTitleSceneRefsAtRuntime(TitleSceneRefs localTitleSceneUIRefs)
    {
        titleSceneRefs = localTitleSceneUIRefs;
    }
}
