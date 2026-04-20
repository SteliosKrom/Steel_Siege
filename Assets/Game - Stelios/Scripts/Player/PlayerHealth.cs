using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public enum PlayerID { P1, P2 }

    [SerializeField] private PlayerID playerID;
    [SerializeField] private int currentLives;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameEventsSO gameEvents;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject[] playerLives;
    #endregion

    private void Start()
    {
        currentLives = playerData.MaxLives;
    }

    public void TakeDamage()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameManager.Instance.PlayerDied(playerID, gameObject);
        }
    }

    public void UpdateLivesUI()
    {
        UIManager.Instance.ManageLivesUI(playerLives, currentLives);
    }
}
