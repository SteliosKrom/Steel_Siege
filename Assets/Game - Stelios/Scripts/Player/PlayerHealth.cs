using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentLives;

    private const string LIVE_TAG = "Live";

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    #region GAME DATA
    [Header("GAME DATA")]
    [SerializeField] private PlayerData playerData;
    #endregion

    public int CurrentLives => currentLives;

    private void Start()
    {
        currentLives = playerData.MaxLives;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(LIVE_TAG))
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
            audioEvents.RaiseGainPowerUp();
            uiEvents.RaiseIncreaseLivesUI(playerData.PlayerType, currentLives);
            obj.SetActive(false);
        }
    }

    public void DecreaseLives()
    {
        audioEvents.RaiseHurt();
        uiEvents.RaiseDecreaseLivesUI(playerData.PlayerType, currentLives);
    }
}
