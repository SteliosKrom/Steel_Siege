using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentLives;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SpawnManager spawnManager;
    #endregion

    #region GAME DATA
    [Header("GAME DATA")]
    [SerializeField] private EnemyData enemyData;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private ScoreEventsSO scoreEvents;
    #endregion

    private void Start()
    {
        currentLives = enemyData.MaxLives;
    }

    public void TakeDamage()
    {
        currentLives--;
        spawnManager.EnemiesAlive--;

        if (currentLives <= 0)
        {
            this.gameObject.SetActive(false);
            scoreEvents.RaiseScoreChanged();
            uiEvents.RaiseScoreUIChanged();
        }
    }
}
