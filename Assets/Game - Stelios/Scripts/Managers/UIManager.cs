using System.Collections;
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
    public MainSceneRefs MainRefs { get => mainRefs; }
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
        uiEvents.OnDecreaseLivesUI += DecreaseLivesUI;
        uiEvents.OnIncreaseLivesUI += IncreaseLivesUI;

        uiEvents.OnShowPVEScore += EnablePVEScoreText;
        uiEvents.OnScoreUIChanged += UpdateScoreUI;
        uiEvents.OnHighScoreUIChanged += UpdateHighScoreUI;
        uiEvents.OnLeaderboardScoresAndNamesUIChanged += UpdateLeaderboardScoresAndNamesUI;

        uiEvents.OnEnableGameModes += EnableGameModes;
        uiEvents.OnEnableWavesUI += EnableEnemyWavesUI;
        uiEvents.OnDisableWavesUI += DisableEnemyWavesUI;

        uiEvents.OnInsertCoin += InsertCoin;
        uiEvents.OnShowEnterYourNamePanel += ShowEnterYourNamePanel;

        uiEvents.OnPVPExit += OnPVPModeExit;
        uiEvents.OnPVPStay += OnPVPModeStay;
        uiEvents.OnPVEExit += OnPVEModeExit;
        uiEvents.OnPVEStay += OnPVEModeStay;

        uiEvents.OnFirstLetterStay += OnFirstLetterStay;
        uiEvents.OnFirstLetterExit += OnFirstLetterExit;
        uiEvents.OnSecondLetterStay += OnSecondLetterStay;
        uiEvents.OnSecondLetterExit += OnSecondLetterExit;
        uiEvents.OnThirdLetterStay += OnThirdLetterStay;
        uiEvents.OnThirdLetterExit += OnThirdLetterExit;

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
        uiEvents.OnDecreaseLivesUI -= DecreaseLivesUI;
        uiEvents.OnIncreaseLivesUI -= IncreaseLivesUI;

        uiEvents.OnShowPVEScore -= EnablePVEScoreText;
        uiEvents.OnScoreUIChanged -= UpdateScoreUI;
        uiEvents.OnHighScoreUIChanged -= UpdateHighScoreUI;
        uiEvents.OnLeaderboardScoresAndNamesUIChanged -= UpdateLeaderboardScoresAndNamesUI;

        uiEvents.OnEnableGameModes -= EnableGameModes;
        uiEvents.OnEnableWavesUI -= EnableEnemyWavesUI;
        uiEvents.OnDisableWavesUI -= DisableEnemyWavesUI;

        uiEvents.OnInsertCoin -= InsertCoin;
        uiEvents.OnShowEnterYourNamePanel -= ShowEnterYourNamePanel;

        uiEvents.OnPVPExit -= OnPVPModeExit;
        uiEvents.OnPVPStay -= OnPVPModeStay;
        uiEvents.OnPVEExit -= OnPVEModeExit;
        uiEvents.OnPVEStay -= OnPVEModeStay;

        uiEvents.OnFirstLetterStay -= OnFirstLetterStay;
        uiEvents.OnFirstLetterExit -= OnFirstLetterExit;
        uiEvents.OnSecondLetterStay -= OnSecondLetterStay;
        uiEvents.OnSecondLetterExit -= OnSecondLetterExit;
        uiEvents.OnThirdLetterStay -= OnThirdLetterStay;
        uiEvents.OnThirdLetterExit -= OnThirdLetterExit;

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

    public void IncreaseLivesUI(PlayerData.PlayerID playerID, int currentLives)
    {
        if (playerID == PlayerData.PlayerID.P1)
            GenericUpdateIncreaseLivesUI(mainRefs.player1Lives, currentLives);
        else if (playerID == PlayerData.PlayerID.P2)
            GenericUpdateIncreaseLivesUI(mainRefs.player2Lives, currentLives);
    }

    public void DecreaseLivesUI(PlayerData.PlayerID playerID, int currentLives)
    {
        if (playerID == PlayerData.PlayerID.P1)
            GenericUpdateDecreaseLivesUI(mainRefs.player1Lives, currentLives);
        else if (playerID == PlayerData.PlayerID.P2)
            GenericUpdateDecreaseLivesUI(mainRefs.player2Lives, currentLives);
    }

    public void GenericUpdateIncreaseLivesUI(GameObject[] lives, int currentLives)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < currentLives)
                lives[i].SetActive(true);
        }
    }

    public void GenericUpdateDecreaseLivesUI(GameObject[] lives, int currentLives)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < currentLives)
                lives[i].SetActive(true);
            else
                lives[i].SetActive(false);
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

    // PVP and PVE Modes selection events...
    public void OnPVPModeStay() => titleRefs.PVPSelectionArrow.color = Color.red;
    public void OnPVPModeExit() => titleRefs.PVPSelectionArrow.color = Color.white;

    public void OnPVEModeStay() => titleRefs.PVESelectionArrow.color = Color.red;
    public void OnPVEModeExit() => titleRefs.PVESelectionArrow.color = Color.white;

    // Enter your name letter highlight events...
    public void OnFirstLetterStay() => mainRefs.firstLetter.color = Color.red;
    public void OnFirstLetterExit() => mainRefs.firstLetter.color = Color.white;

    public void OnSecondLetterStay() => mainRefs.secondLetter.color = Color.red;
    public void OnSecondLetterExit() => mainRefs.secondLetter.color = Color.white;

    public void OnThirdLetterStay() => mainRefs.thirdLetter.color = Color.red;
    public void OnThirdLetterExit() => mainRefs.thirdLetter.color = Color.white;

    // Enable Panels on events...
    public void ShowDraw() => mainRefs.drawPanel.SetActive(true);
    public void ShowP1Win() => mainRefs.player1WinsPanel.SetActive(true);
    public void ShowP2Win() => mainRefs.player2WinsPanel.SetActive(true);
    public void ShowGameOver() => mainRefs.gameOverPanel.SetActive(true);
    public void ShowEnterYourNamePanel() => mainRefs.enterYourNamePanel.SetActive(true);

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

    // Update Score, Highscore and Leaderboard Scores and Names...
    public void UpdateScoreUI() => mainRefs.scoreTextValue.text = ScoreManager.Instance.CurrentScore.ToString("000000");
    public void UpdateHighScoreUI() => titleRefs.highscoreText.text = ScoreManager.Instance.CurrentHighScore.ToString("000000");
    public void UpdateLeaderboardScoresAndNamesUI()
    {
        for (int i = 0; i <= LeaderboardManager.Instance.LeaderboardRefs.bestScoresText.Length - 1; i++)
        {
            if (i < LeaderboardManager.Instance.BestScores.Count)
            {
                LeaderboardManager.Instance.LeaderboardRefs.bestScoresText[i].text = LeaderboardManager.Instance.BestScores[i].ToString("000000");
                LeaderboardManager.Instance.LeaderboardRefs.bestScoreNamesText[i].text = LeaderboardManager.Instance.BestScoreNames[i];
            }
            else
            {
                LeaderboardManager.Instance.LeaderboardRefs.bestScoresText[i].text = "000000";
                LeaderboardManager.Instance.LeaderboardRefs.bestScoreNamesText[i].text = LeaderboardManager.Instance.BestScoreNames[i];
            }
        }
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
