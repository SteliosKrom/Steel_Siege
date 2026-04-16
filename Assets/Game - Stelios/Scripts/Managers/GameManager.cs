using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool player1Dead = false;
    private bool player2Dead = false;

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
        currentModeIndex = 0;
        currentGameState = GameState.Title;
        currentPlayerState = PlayerState.Idle;
        currentGameMode = GameMode.None;
    }

    private void Update()
    {
        JoinGame();
        InputForGameModeSelection();
        EnterGameMode();
    }

    public void JoinGame()
    {
        if (UIManager.Instance.IsWaiting) return;
        if (currentGameState == GameState.SelectModes) return;

        // Change later to Arcade Machine input...
        if (currentGameState == GameState.Title)
        {
            if (Input.anyKeyDown)
            {
                switch (UIManager.Instance.CreditCounter)
                {
                    case 0:
                        UIManager.Instance.InsertCoin();
                        break;
                    case 1:
                        currentGameState = GameState.SelectModes;
                        UIManager.Instance.EnableGameModes();
                        UIManager.Instance.TriggerPVPStay();
                        break;
                }
            }
        }
    }

    public void EnterGameMode()
    {
        if (IsOnAnyGameState()) return;

        if (currentGameState == GameState.SelectModes)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentModeIndex == 0)
                    LoadPVPMode();
                else
                    LoadPVEMode();
            }
        }
    }

    public void InputForGameModeSelection()
    {
        if (IsOnAnyGameState()) return;

        // Change later to Arcade Machine inputs...
        if (Input.GetKeyDown(KeyCode.S) && currentModeIndex == 0)
        {
            SelectPVEMode();
        }
        else if (Input.GetKeyDown(KeyCode.W) && currentModeIndex == 1)
        {
            SelectPVPMode();
        }
    }

    public void SelectPVPMode()
    {
        UIManager.Instance.TriggerPVEExit();
        UIManager.Instance.OnPVPSelected();
        UIManager.Instance.TriggerPVPStay();
        currentModeIndex = 0;
    }

    public void SelectPVEMode()
    {
        UIManager.Instance.TriggerPVPExit();
        UIManager.Instance.OnPVESelected();
        UIManager.Instance.TriggerPVEStay();
        currentModeIndex = 1;
    }

    public void LoadPVPMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVP;
        SceneManager.LoadScene("Main");
        UIManager.Instance.EnablePlayers();
    }

    public void LoadPVEMode()
    {
        currentGameState = GameState.Playing;
        currentGameMode = GameMode.PVE;
        SceneManager.LoadScene("Main");
    }

    public void PlayerDied(PlayerHealth.PlayerID id, GameObject obj)
    {
        if (id == PlayerHealth.PlayerID.P1)
            player1Dead = true;
        else if (id == PlayerHealth.PlayerID.P2)
            player2Dead = true;

        obj.SetActive(false);
        StartCoroutine(CheckGameResultDelay());
    }

    public void CheckGameResult()
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

    public void Draw()
    {
        UIManager.Instance.ShowDraw();
        currentGameState = GameState.Draw;
    }

    public void Player1Wins()
    {
        UIManager.Instance.ShowP1Win();
        currentGameState = GameState.Player1Wins;
    }

    public void Player2Wins()
    {
        UIManager.Instance.ShowP2Win();
        currentGameState = GameState.Player2Wins;
    }

    public void ReturnToMainTitle()
    {
        StartCoroutine(ReturnToMainTitleDelay());
    }

    public bool IsOnAnyGameState()
    {
        return currentGameState == GameState.Title
            || currentGameState == GameState.Playing
            || currentGameState == GameState.Player1Wins
            || currentGameState == GameState.Player2Wins
            || currentGameState == GameState.Draw
            || currentGameState == GameState.GameOver
            || currentGameState == GameState.Leaderboard;
    }

    public IEnumerator ReturnToMainTitleDelay()
    {
        yield return new WaitForSeconds(returnBackToMainTitleDelay);
        SceneManager.LoadScene("Title");
        player1Dead = false;
        player2Dead = false;
        currentGameState = GameState.Title;
    }

    public IEnumerator CheckGameResultDelay()
    {
        yield return new WaitForSeconds(checkGameResultDelay);
        CheckGameResult();
    }
}
