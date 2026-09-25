using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform VFXParent;
    [SerializeField] private string poolID;
    [SerializeField] private int currentLives;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject explosionPrefab;
    #endregion

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
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    private void OnEnable()
    {
        ResetLives();
    }

    public void ResetLives()
    {
        currentLives = enemyData.MaxLives;
    }

    public void TakeDamage()
    {
        currentLives--;
        spawnManager.EnemiesAlive--;

        if (currentLives <= 0)
        {

            scoreEvents.RaiseScoreChanged();
            uiEvents.RaiseScoreUIChanged();
            audioEvents.RaiseExplosion();

            Instantiate(explosionPrefab, transform.position, Quaternion.identity, VFXParent);

            ReturnEnemy();
        }
    }

    public void ReturnEnemy()
    {
        ObjectPoolManager.Instance.ReturnObject(poolID, this.gameObject);
    }
}
