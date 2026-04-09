using NUnit.Framework.Internal.Filters;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title, 
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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float checkGameResultDelay = 0.1f;
    private float returnBackToMainTitleDelay = 3f;

    private bool player1Dead = false;
    private bool player2Dead = false;

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private PlayerState currentPlayerState;
    #endregion

    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }

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
        currentGameState = GameState.Title;
        currentPlayerState = PlayerState.Idle;
    }

    private void Update()
    {
        JoinGame();
    }

    public void JoinGame()
    {
        if (UIManager.Instance.IsWaiting) return;

        // Change to Joystick button 7 later
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
                        SceneManager.LoadScene("Main");
                        currentGameState = GameState.Playing;
                        break;
                }
            }
        }
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
        UIManager.Instance.MainSceneUIRefs.drawPanel.SetActive(true);
        currentGameState = GameState.Draw;
    }

    public void Player1Wins()
    {
        UIManager.Instance.MainSceneUIRefs.player1WinsPanel.SetActive(true);
        currentGameState = GameState.Player1Wins;
    }

    public void Player2Wins()
    {
        UIManager.Instance.MainSceneUIRefs.player2WinsPanel.SetActive(true);
        currentGameState = GameState.Player2Wins;
    }

    public void ReturnToMainTitle()
    {
        StartCoroutine(ReturnToMainTitleDelay());
    }

    public IEnumerator ReturnToMainTitleDelay()
    {
        yield return new WaitForSeconds(returnBackToMainTitleDelay);
        SceneManager.LoadScene("Title");
        player1Dead = false;
        player2Dead = false;
        CurrentGameState = GameState.Title;
    }

    public IEnumerator CheckGameResultDelay()
    {
        yield return new WaitForSeconds(checkGameResultDelay);
        CheckGameResult();
    }
}
