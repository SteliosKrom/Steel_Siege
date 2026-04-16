using UnityEngine;

[System.Serializable]
public class MainSceneRefs
{
    public GameObject gameOverPanel;
    public GameObject player1WinsPanel;
    public GameObject player2WinsPanel;
    public GameObject drawPanel;
    public GameObject player1;
    public GameObject player2;
}

public class MainSceneBridge : MonoBehaviour
{
    #region LOCAL REFERENCES
    [Header("LOCAL REFERENCES")]
    [SerializeField] private MainSceneRefs mainUI;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioManager.SoundType[] soundTypes;
    [SerializeField] private AudioSource[] localSources;
    #endregion

    private void Awake()
    {
        AudioManager.Instance.SetAudioSources(soundTypes, localSources);
        UIManager.Instance.SetMainUI(mainUI);
    }
}
