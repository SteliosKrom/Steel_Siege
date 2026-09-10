using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float timerCount;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject pvpTimer;
    #endregion

    #region UI
    [SerializeField] private TextMeshProUGUI pvpTimerText;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerHealth player1HP;
    [SerializeField] private PlayerHealth player2HP;
    #endregion

    #region PROPERTIES
    public float TimerCount
    {
        get => timerCount;
        set => timerCount = Mathf.Max(0, value);
    }
    #endregion

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing)
            return;

        CountTime();
    }

    private void CountTime()
    {
        TimerCount -= Time.deltaTime;
        pvpTimerText.text = TimerCount.ToString("0");

        if (timerCount == 0)
        {
            if (player1HP.CurrentLives < player2HP.CurrentLives)
            {
                GameManager.Instance.Player2Wins();
            }
            else if (player2HP.CurrentLives < player1HP.CurrentLives)
            {
                GameManager.Instance.Player1Wins();
            }
            else
            {
                GameManager.Instance.Draw();
            }
            pvpTimer.SetActive(false);
            StartCoroutine(GameManager.Instance.CheckGameResultDelay());
        }
    }
}
