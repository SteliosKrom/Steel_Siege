using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title, 
    Playing, 
    GameOver, 
    Leaderboard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    #endregion

    public GameState CurrentGameState { get; set; }

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
    }

    private void Update()
    {
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
}
