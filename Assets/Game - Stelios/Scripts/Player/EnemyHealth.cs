using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentLives;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SpawnManager spawnManager;
    #endregion

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private EnemyData enemyData;
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
        }
    }
}
