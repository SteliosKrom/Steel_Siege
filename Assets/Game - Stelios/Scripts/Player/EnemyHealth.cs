using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private const string ENEMY_TAG = "Enemy";
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
            ReturnEnemy();
            scoreEvents.RaiseScoreChanged();
            uiEvents.RaiseScoreUIChanged();
        }
    }

    public void ReturnEnemy()
    {
        ObjectPoolManager.Instance.ReturnObject(ENEMY_TAG, this.gameObject);
    }
}
