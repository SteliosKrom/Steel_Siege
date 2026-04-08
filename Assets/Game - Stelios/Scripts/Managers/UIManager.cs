using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private float enterCreditDelay = 2f;
    private int creditCounter = 0;
    private bool isWaiting = false;

    #region UI
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI insertCoinText;
    [SerializeField] private TextMeshProUGUI pressStartText;
    [SerializeField] private TextMeshProUGUI creditText;
    #endregion

    [SerializeField] private GameEvents gameEvents;
    [SerializeField] private GameObject gameOverPanel;

    public GameObject GameOverPanel { get => gameOverPanel; set => gameOverPanel = value; }
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
        insertCoinText.enabled = true;
        pressStartText.enabled = false;
        creditText.text = creditCounter.ToString("00");
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        isWaiting = true;
        insertCoinText.enabled = false;
        CreditCounter++;
        creditText.text = creditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        pressStartText.enabled = true;
        isWaiting = false;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
