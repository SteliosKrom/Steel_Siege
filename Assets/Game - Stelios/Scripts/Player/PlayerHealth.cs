using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentLives;

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    #endregion

    #region GAME DATA
    [Header("GAME DATA")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PowerUpData powerUpData;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject[] playerLives;
    #endregion

    private void Start()
    {
        currentLives = playerData.MaxLives;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (powerUpData.PowerUpType == PowerUpData.Type.Lives)
        {
            IncreaseLives(other.gameObject);
        }
    }

    public void TakeDamage()
    {
        currentLives--;
        DecreaseLives();

        if (currentLives <= 0)
        {
            GameManager.Instance.PlayerDied(playerData.PlayerType, gameObject);
        }
    }

    public void IncreaseLives(GameObject obj)
    {
        if (currentLives == playerData.MaxLives)
        {
            // You can't increase your lives, because you have max lives. Give feedback to the player...
            return;
        }
        else
        {
            currentLives++;
            UIManager.Instance.IncreaseLivesUI(playerLives, currentLives);
            obj.SetActive(false);
        }
    }

    public void DecreaseLives()
    {
        UIManager.Instance.DecreaseLivesUI(playerLives, currentLives);
    }
}
