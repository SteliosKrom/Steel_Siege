using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public enum GameState
{
    Title,
    SelectModes,
    Playing,
    GameOver,
    Player1Wins,
    Player2Wins,
    Draw,
    Leaderboard
}

public enum GameMode
{
    None,
    PVP,
    PVE
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentModeIndex;
    private int currentLetterIndex = 0;

    private float checkGameResultDelay = 0.1f;
    private float returnBackToMainTitleDelay = 3f;
    private float moveToEnterYourNameDelay = 3f;
    private float returnBackToMainTitleFromLeaderboard = 10f;

    private bool player1Dead = false;
    private bool player2Dead = false;
    private bool onEnterYourName = false;

    private char[] allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private string currentLeaderboardBestScorePlayerName;

    #region INPUT
    private PlayerControls playerControls;
    private Vector2 navigateInput;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private ScoreEventsSO scoreEvents;
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private GameMode currentGameMode;
    #endregion

    public string CurrentLeaderboardBestScorePlayerName { get => currentLeaderboardBestScorePlayerName; set => currentLeaderboardBestScorePlayerName = value; }
    public int CurrentModeIndex { get => currentModeIndex; }
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public GameMode CurrentGameMode { get => currentGameMode; set => currentGameMode = value; }

    private void Awake()
    {
        playerControls = new PlayerControls();

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
        playerControls.Enable();
        playerControls.UI.Navigate.started += OnNavigate;
        playerControls.UI.AnyInput.started += OnAnyKey;
        playerControls.UI.Submit.started += OnSubmit;
    }

    private void OnDisable()
    {
        playerControls.UI.Navigate.started -= OnNavigate;
        playerControls.UI.AnyInput.started -= OnAnyKey;
        playerControls.UI.Submit.started -= OnSubmit;
        playerControls.Disable();
    }

    private void Start()
    {
        currentModeIndex = 0;
        currentGameState = GameState.Title;
        currentGameMode = GameMode.None;
    }

    private void Update()
    {
        InputForGameModeSelection();
        EnterYourName();
    }

    public void OnNavigate(InputAction.CallbackContext cxt)
    {
        navigateInput = cxt.ReadValue<Vector2>();
    }

    public void OnSubmit(InputAction.CallbackContext cxt)
    {
        EnterGameMode();
    }

    public void OnAnyKey(InputAction.CallbackContext cxt)
    {
        JoinGame();
    }

    public void JoinGame()
    {
        if (UIManager.Instance.IsWaiting) return;
        if (currentGameState == GameState.SelectModes) return;

        // Change later to Arcade Machine input...
        if (currentGameState == GameState.Title)
        {
            switch (UIManager.Instance.CreditCounter)
            {
                case 0:
                    uiEvents.RaiseInsertCoin();
                    break;
                case 1:
                    currentGameState = GameState.SelectModes;
                    uiEvents.RaiseEnableGameModes();
                    uiEvents.RaisePVPStay();
                    break;
            }
        }
    }

    public void EnterGameMode()
    {
        if (currentGameState != GameState.SelectModes) return;

        if (currentGameState == GameState.SelectModes)
        {
            if (currentModeIndex == 0)
                LoadPVPMode();
            else
                LoadPVEMode();
        }
    }

    public void InputForGameModeSelection()
    {
        if (currentGameState != GameState.SelectModes) return;

        // Change later to Arcade Machine inputs...
        if (navigateInput.y < 0 && currentModeIndex == 0)
            SelectPVEMode();
        else if (navigateInput.y > 0 && currentModeIndex == 1)
            SelectPVPMode();

        navigateInput = Vector2.zero;
    }

    public void SelectPVPMode()
    {
        uiEvents.RaisePVEExit();
        uiEvents.RaisePVPStay();
        uiEvents.RaisePVPSelected();
        currentModeIndex = 0;
    }

    public void SelectPVEMode()
    {
        uiEvents.RaisePVPExit();
        uiEvents.RaisePVEStay();
        uiEvents.RaisePVESelected();
        currentModeIndex = 1;
    }

    public void LoadPVPMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVP;
        ScoreManager.Instance.ResetRunScores();
        StartCoroutine(RaiseAfterLoad(true));
        uiEvents.RaiseEnablePVPLives();
    }

    public void LoadPVEMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVE;
        ScoreManager.Instance.ResetRunScores();
        StartCoroutine(RaiseAfterLoad(false));
        uiEvents.RaiseEnablePVELives();
        uiEvents.RaiseShowPVEScore();
        uiEvents.RaiseHideRedLives();
    }

    public void PlayerDied(PlayerData.PlayerID id, GameObject obj)
    {
        if (id == PlayerData.PlayerID.P1)
            player1Dead = true;
        else if (id == PlayerData.PlayerID.P2)
            player2Dead = true;

        obj.SetActive(false);
        StartCoroutine(CheckGameResultDelay());
    }

    public void CheckGameResult()
    {
        if (currentGameMode == GameMode.PVP)
        {
            if (player1Dead && player2Dead)
            {
                Draw();
            }
            else if (player1Dead)
            {
                Player2Wins();
            }
            else if (player2Dead)
            {
                Player1Wins();
            }
            StartCoroutine(ReturnToMainTitleDelay());
        }
        else if (currentGameMode == GameMode.PVE)
        {
            if (player1Dead)
            {
                GameOver();
                StartCoroutine(MoveToEnterYourNamePanelDelay());
            }
        }
    }

    public void Draw()
    {
        gameEvents.RaiseDraw();
        currentGameState = GameState.Draw;
    }

    public void Player1Wins()
    {
        gameEvents.RaiseP1Win();
        currentGameState = GameState.Player1Wins;
    }

    public void Player2Wins()
    {
        gameEvents.RaiseP2Win();
        currentGameState = GameState.Player2Wins;
    }

    public void GameOver()
    {
        gameEvents.RaiseGameOver();
        audioEvents.RaiseGameOver();
        currentGameState = GameState.GameOver;
    }

    public void EnterYourName()
    {
        if (!onEnterYourName) return;
        if (currentLetterIndex >= UIManager.Instance.MainRefs.letters.Length) return;

        for (int i = 0; i < allowedChars.Length; i++)
        { 
            if (Input.GetKeyDown(KeyCode.A + i))
            {
                UIManager.Instance.MainRefs.letters[currentLetterIndex].text = allowedChars[i].ToString();
                currentLetterIndex++;

                currentLeaderboardBestScorePlayerName = string.Concat(UIManager.Instance.MainRefs.letters[0].text, 
                    UIManager.Instance.MainRefs.letters[1].text, 
                    UIManager.Instance.MainRefs.letters[2].text);

                if (currentLetterIndex == 3)
                {
                    int finalScore = ScoreManager.Instance.CurrentScore;
                    string playerName = currentLeaderboardBestScorePlayerName;

                    LeaderboardManager.Instance.InsertScore(finalScore, playerName);
                    SaveManager.Instance.SaveAll();

                    StartCoroutine(MoveToLeaderboardDelay());
                }

                break;
            }
        }

        switch (currentLetterIndex)
        {
            case 1:
                uiEvents.RaiseFirstLetterExit();
                uiEvents.RaiseSecondLetterStay();
                break;
            case 2:
                uiEvents.RaiseSecondLetterExit();
                uiEvents.RaiseThirdLetterStay();
                break;
        }
    }

    public void ResetMatchState()
    {
        switch (currentGameMode)
        {
            case GameMode.PVP:
                player1Dead = false;
                player2Dead = false;
                break;
            case GameMode.PVE:
                player1Dead = false;
                break;
        }
    }

    public IEnumerator RaiseAfterLoad(bool isPVP)
    {
        SceneManager.LoadScene("Main");
        yield return null;

        if (isPVP)
            gameEvents.RaisePVPLoad();
        else
            gameEvents.RaisePVELoad();
    }

    public IEnumerator MoveToEnterYourNamePanelDelay()
    {
        yield return new WaitForSeconds(moveToEnterYourNameDelay);
        onEnterYourName = true;
        UIManager.Instance.MainRefs.gameOverPanel.SetActive(false);
        uiEvents.RaiseShowEnterYourNamePanel();
        uiEvents.RaiseFirstLetterStay();
    }

    public IEnumerator MoveToLeaderboardDelay()
    {
        yield return null;

        SceneManager.LoadScene("Leaderboard");
        currentGameState = GameState.Leaderboard;

        yield return null;
        uiEvents.RaiseLeaderboardScoresAndNamesUIChanged();

        yield return new WaitForSeconds(returnBackToMainTitleFromLeaderboard);

        SceneManager.LoadScene("Title");

        yield return null;
        uiEvents.RaiseHighScoreUIChanged();
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator ReturnToMainTitleDelay()
    {
        yield return new WaitForSeconds(returnBackToMainTitleDelay);
        SceneManager.LoadScene("Title");

        yield return null;
        uiEvents.RaiseHighScoreUIChanged();
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator CheckGameResultDelay()
    {
        yield return new WaitForSeconds(checkGameResultDelay);
        CheckGameResult();
    }
}
