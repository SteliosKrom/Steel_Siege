using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;
    [SerializeField] private GameObject gameOverPanel;

    public GameObject GameOverPanel => gameOverPanel;

    private void OnEnable()
    {
        gameEvents.OnGameOver.AddListener(ShowGameOver);
    }

    private void OnDisable()
    {
        gameEvents.OnGameOver.RemoveListener(ShowGameOver);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
