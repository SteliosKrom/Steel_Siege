using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentScore;
    private int currentHighScore;

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private ScoreEventsSO scoreEvents;
    #endregion

    #region GAME DATA 
    [Header("GAME DATA")]
    [SerializeField] private EnemyData enemyData;
    #endregion

    public int CurrentScore => currentScore;
    public int CurrentHighScore { get => currentHighScore; set => currentHighScore = value; }

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
        scoreEvents.OnScoreChanged += UpdateScore;
        scoreEvents.OnHighScoreChanged += UpdateHighScore;
    }

    private void OnDisable()
    {
        scoreEvents.OnScoreChanged -= UpdateScore;
        scoreEvents.OnHighScoreChanged -= UpdateHighScore;
    }

    public void UpdateScore()
    {
        currentScore += enemyData.ScoreValue;
        UpdateHighScore();
    }

    public void UpdateHighScore()
    {
        if (currentScore > currentHighScore)
        {
            currentHighScore = currentScore;
            SaveManager.Instance.SaveHighscore();
        }
    }
}
