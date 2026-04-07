using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentLives;

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameEvents gameEvents;
    #endregion

    [SerializeField] private GameObject[] playerLives;

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
            GameManager.Instance.CurrentGameState = GameState.GameOver;
            gameEvents.RaiseGameOver();
        }
    }

    public void UpdateLivesUI()
    {
        for (int i = 0; i < playerLives.Length; i++)
        {
            if (i < currentLives)
            {
                playerLives[i].SetActive(true);
            }
            else
            {
                playerLives[i].SetActive(false);
            }
        }
    }
}
