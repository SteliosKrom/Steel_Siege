using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private MainSceneUIRefs mainSceneUIRefs;
    [SerializeField] private TitleSceneUIRefs titleSceneUIRefs;

    private float enterCreditDelay = 2f;
    private int creditCounter = 0;
    private bool isWaiting = false;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private GameEvents gameEvents;
    #endregion

    public TitleSceneUIRefs TitleSceneUIRefs { get => titleSceneUIRefs; }
    public MainSceneUIRefs MainSceneUIRefs { get => mainSceneUIRefs; } 
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
        titleSceneUIRefs.insertCoinText.enabled = true;
        titleSceneUIRefs.pressStartText.enabled = false;
        titleSceneUIRefs.creditCoinText.text = creditCounter.ToString("00");
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        isWaiting = true;
        titleSceneUIRefs.insertCoinText.enabled = false;
        CreditCounter++;
        titleSceneUIRefs.creditCoinText.text = creditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        titleSceneUIRefs.pressStartText.enabled = true;
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
        mainSceneUIRefs.gameOverPanel.SetActive(true);
    }

    public void AssignMainSceneUIRefsAtRuntime(MainSceneUIRefs localMainSceneUIRefs)
    {
        mainSceneUIRefs = localMainSceneUIRefs;
    }

    public void AssignTitleSceneUIRefsAtRuntime(TitleSceneUIRefs localTitleSceneUIRefs)
    {
        titleSceneUIRefs = localTitleSceneUIRefs;
    }
}
