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
            GameManager.Instance.PlayerDied(playerData.PlayerType, gameObject);
        }
    }

    // Change the method name to DecreaseLivesUI and add it as an event to be triggered in UI Manager. Add a new Increase Lives UI same as Decrease Method...
    public void UpdateLivesUI()
    {
        UIManager.Instance.ManageLivesUI(playerLives, currentLives);
    }
}
