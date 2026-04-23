using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private MainSceneRefs mainRefs;
    [SerializeField] private TitleSceneRefs titleRefs;

    private float enterCreditDelay = 2f;
    private float assignRefsDelay = 0.1f;

    private int creditCounter = 0;

    private bool isWaiting = false;

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private UIEventsSO uiEvents;
    #endregion

    public TitleSceneRefs TitleRefs { get => titleRefs; }
    public MainSceneRefs MainRefs{ get => mainRefs; }
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

    private void Start()
    {
        if (titleRefs == null) return;

        titleRefs.insertCoinText.enabled = true;
        titleRefs.pressStartText.enabled = false;
        titleRefs.creditCoinText.text = creditCounter.ToString("00");
    }

    private void OnEnable()
    {
        // Subscribe Game Events...
        gameEvents.OnGameOver += ShowGameOver;
        gameEvents.OnP1Win += ShowP1Win;
        gameEvents.OnP2Win += ShowP2Win;
        gameEvents.OnDraw += ShowDraw;

        // Subscribe UI Events...
        uiEvents.OnHideRedLives += DisableRedLives;
        uiEvents.OnEnablePVPLives += EnablePVPLives;
        uiEvents.OnEnablePVELives += EnablePVELives;
        uiEvents.OnShowPVEScore += EnablePVEScoreText;
        uiEvents.OnScoreUIChanged += UpdateScoreUI;
        uiEvents.OnHighScoreUIChanged += UpdateHighScoreUI;
        uiEvents.OnEnableGameModes += EnableGameModes;
        uiEvents.OnEnableWavesUI += EnableEnemyWavesUI;
        uiEvents.OnDisableWavesUI += DisableEnemyWavesUI;
        uiEvents.OnInsertCoin += InsertCoin;

        uiEvents.OnPVPExit += OnPVPModeExit;
        uiEvents.OnPVPStay += OnPVPModeStay;
        uiEvents.OnPVEExit += OnPVEModeExit;
        uiEvents.OnPVEStay += OnPVEModeStay;

        uiEvents.OnPVPSelected += OnPVPSelected;
        uiEvents.OnPVESelected += OnPVESelected;
    }

    private void OnDisable()
    {
        // Un-Subscribe Game Events...
        gameEvents.OnGameOver -= ShowGameOver;
        gameEvents.OnP1Win -= ShowP1Win;
        gameEvents.OnP2Win -= ShowP2Win;
        gameEvents.OnDraw -= ShowDraw;

        // Un-Subscribe UI Events...
        uiEvents.OnHideRedLives -= DisableRedLives;
        uiEvents.OnEnablePVPLives -= EnablePVPLives;
        uiEvents.OnEnablePVELives -= EnablePVELives;
        uiEvents.OnShowPVEScore -= EnablePVEScoreText;
        uiEvents.OnScoreUIChanged -= UpdateScoreUI;
        uiEvents.OnHighScoreUIChanged -= UpdateHighScoreUI;
        uiEvents.OnEnableGameModes -= EnableGameModes;
        uiEvents.OnEnableWavesUI -= EnableEnemyWavesUI;
        uiEvents.OnDisableWavesUI -= DisableEnemyWavesUI;
        uiEvents.OnInsertCoin -= InsertCoin;

        uiEvents.OnPVPExit -= OnPVPModeExit;
        uiEvents.OnPVPStay -= OnPVPModeStay;
        uiEvents.OnPVEExit -= OnPVEModeExit;
        uiEvents.OnPVEStay -= OnPVEModeStay;

        uiEvents.OnPVPSelected -= OnPVPSelected;
        uiEvents.OnPVESelected -= OnPVESelected;
    }

    public void EnableGameModes()
    {
        titleRefs.modesMenu.SetActive(true);
        titleRefs.PVPText.enabled = true;
        titleRefs.PVEText.enabled = true;
        titleRefs.PVPSelectionArrow.enabled = true;
        titleRefs.pressStartText.enabled = false;
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        isWaiting = true;
        titleRefs.insertCoinText.enabled = false;
        CreditCounter++;
        titleRefs.creditCoinText.text = creditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        titleRefs.pressStartText.enabled = true;
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
        titleRefs.PVPSelectionArrow.enabled = true;
        titleRefs.PVESelectionArrow.enabled = false;
    }

    public void OnPVESelected()
    {
        titleRefs.PVPSelectionArrow.enabled = false;
        titleRefs.PVESelectionArrow.enabled = true;
    }

    public void OnPVPModeStay()
    {
        titleRefs.PVPSelectionArrow.color = Color.red;
    }

    public void OnPVPModeExit()
    {
        titleRefs.PVPSelectionArrow.color = Color.white;
    }

    public void OnPVEModeStay()
    {
        titleRefs.PVESelectionArrow.color = Color.red;
    }

    public void OnPVEModeExit()
    {
        titleRefs.PVESelectionArrow.color = Color.white;
    }

    public void ShowDraw()
    {
        mainRefs.drawPanel.SetActive(true);
    }

    public void ShowP1Win()
    {
        mainRefs.player1WinsPanel.SetActive(true);
    }

    public void ShowP2Win()
    {
        mainRefs.player2WinsPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        mainRefs.gameOverPanel.SetActive(true);
    }

    public void EnableEnemyWavesUI()
    {
        mainRefs.wavesText.enabled = true;
        mainRefs.wavesCountText.enabled = true;
    }

    public void DisableEnemyWavesUI()
    {
        mainRefs.wavesText.enabled = false;
        mainRefs.wavesCountText.enabled = false;
    }

    public void UpdateScoreUI()
    {
        mainRefs.scoreTextValue.text = ScoreManager.Instance.CurrentScore.ToString("000000");
    }

    public void UpdateHighScoreUI()
    {
        titleRefs.highscoreText.text = ScoreManager.Instance.CurrentHighScore.ToString("000000");
    }

    public void EnablePVEScoreText()
    {
        StartCoroutine(EnableScoreTextDelay());
    }

    public void EnablePVELives()
    {
        StartCoroutine(EnablePVELivesDelay());
    }

    public void EnablePVPLives()
    {
        StartCoroutine(EnablePVPLivesDelay());
    }

    public void DisableRedLives()
    {
        StartCoroutine(DisableRedLivesDelay());
    }

    public IEnumerator EnablePVPLivesDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        mainRefs.redLives.SetActive(true);
        mainRefs.greenLives.SetActive(true);
    }

    public IEnumerator EnablePVELivesDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        mainRefs.greenLives.SetActive(true);
    }

    public IEnumerator DisableRedLivesDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        mainRefs.redLives.SetActive(false);
    }

    public IEnumerator EnableScoreTextDelay()
    {
        yield return new WaitForSeconds(assignRefsDelay);
        mainRefs.scoreText.enabled = true;
        mainRefs.scoreTextValue.enabled = true;
    }

    public void SetMainUI(MainSceneRefs localMainRefs)
    {
        mainRefs = localMainRefs;
    }

    public void SetTitleUI(TitleSceneRefs localTitleRefs)
    {
        titleRefs = localTitleRefs;
    }
}
