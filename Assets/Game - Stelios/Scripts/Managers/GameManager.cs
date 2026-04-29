using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

public enum PlayerState
{
    Idle,
    Moving
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

    private float checkGameResultDelay = 0.1f;
    private float returnBackToMainTitleDelay = 3f;
    private float moveToLeaderboardDelay = 3f;
    private float returnBackToMainTitleFromLeaderboard = 10f;

    private bool player1Dead = false;
    private bool player2Dead = false;

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
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private GameMode currentGameMode;
    #endregion

    public int CurrentModeIndex { get => currentModeIndex; }
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }
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
        currentPlayerState = PlayerState.Idle;
        currentGameMode = GameMode.None;
    }

    private void Update()
    {
        InputForGameModeSelection();
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
        StartCoroutine(RaiseAfterLoad(true));
        uiEvents.RaiseEnablePVPLives();
    }

    public void LoadPVEMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVE;
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
            ReturnToMainTitle();
        }
        else if (currentGameMode == GameMode.PVE)
        {
            if (player1Dead)
            {
                GameOver();
            }
            MoveToLeaderboardScene();
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
        scoreEvents.RaiseHighScoreChanged();
        audioEvents.RaiseGameOver();
        LeaderboardManager.Instance.ProcessLeaderboard();
        currentGameState = GameState.GameOver;
    }

    public void MoveToLeaderboardScene()
    {
        StartCoroutine(MoveToLeaderboardDelay());
    }

    public void ReturnToMainTitle()
    {
        StartCoroutine(ReturnToMainTitleDelay());
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

    public IEnumerator MoveToLeaderboardDelay()
    {
        yield return new WaitForSeconds(moveToLeaderboardDelay);

        SceneManager.LoadScene("Leaderboard");
        currentGameState = GameState.Leaderboard;

        yield return new WaitForSeconds(returnBackToMainTitleFromLeaderboard);

        SceneManager.LoadScene("Title");
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator ReturnToMainTitleDelay()
    {
        yield return new WaitForSeconds(returnBackToMainTitleDelay);
        SceneManager.LoadScene("Title");
        ResetMatchState();
        currentGameState = GameState.Title;
    }

    public IEnumerator CheckGameResultDelay()
    {
        yield return new WaitForSeconds(checkGameResultDelay);
        CheckGameResult();
    }
}
