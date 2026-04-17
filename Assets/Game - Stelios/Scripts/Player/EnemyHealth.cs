using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private int currentLives;

    private void Start()
    {
        currentLives = enemyData.MaxLives;
    }

    public void TakeDamage()
    {
        currentLives--;

        if (currentLives <= 0)
            this.gameObject.SetActive(false);
    }
}
