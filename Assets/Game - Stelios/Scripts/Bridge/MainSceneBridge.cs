using TMPro;
using UnityEngine;

[System.Serializable]
public class MainSceneRefs
{
    public GameObject[] player1Lives;
    public GameObject[] player2Lives;

    public GameObject gameOverPanel;
    public GameObject enterYourNamePanel;
    public GameObject player1WinsPanel;
    public GameObject player2WinsPanel;
    public GameObject drawPanel;

    public GameObject redLives;
    public GameObject greenLives;

    public GameObject player1;
    public GameObject player2;
    public GameObject enemyTank;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextValue;
    public TextMeshProUGUI wavesText;
    public TextMeshProUGUI wavesCountText;
    public TextMeshProUGUI[] letters;
    public TextMeshProUGUI firstLetter;
    public TextMeshProUGUI secondLetter;
    public TextMeshProUGUI thirdLetter;
}

public class MainSceneBridge : MonoBehaviour
{
    #region LOCAL REFERENCES
    [Header("LOCAL REFERENCES")]
    [SerializeField] private MainSceneRefs mainRefs;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioManager.SoundType[] soundTypes;
    [SerializeField] private AudioSource[] localSources;
    #endregion

    private void Awake()
    {
        AudioManager.Instance.SetAudioSources(soundTypes, localSources);
        UIManager.Instance.SetMainUI(mainRefs);
    }
}
