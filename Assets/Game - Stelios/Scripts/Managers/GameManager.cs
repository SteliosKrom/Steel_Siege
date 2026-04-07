using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title, 
    Playing, 
    GameOver, 
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
        if (TitleSceneUIManager.Instance.IsWaiting) return;

        // Change to Joystick button 7 later
        if (currentGameState == GameState.Title)
        {
            if (Input.anyKeyDown)
            {
                switch (TitleSceneUIManager.Instance.CreditCounter)
                {
                    case 0:
                        TitleSceneUIManager.Instance.InsertCoin();
                        break;
                    case 1:
                        SceneManager.LoadScene("Main");
                        currentGameState = GameState.Playing;
                        break;
                }
            }
        }
    }
}
