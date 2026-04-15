using UnityEngine;

[System.Serializable]
public class MainSceneRefs
{
    public GameObject gameOverPanel;
    public GameObject player1WinsPanel;
    public GameObject player2WinsPanel;
    public GameObject drawPanel;
    public GameObject player_1;
    public GameObject player_2;
}

public class MainSceneBridge : MonoBehaviour
{
    #region LOCAL REFERENCES
    [Header("LOCAL REFERENCES")]
    [SerializeField] private MainSceneRefs localMainSceneRefs;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioManager.SoundType[] soundTypes;
    [SerializeField] private AudioSource[] localSources;
    #endregion

    private void Start()
    {
        GameManager.Instance.AssignMainSceneObjectsAtRuntime(localMainSceneRefs);
        AudioManager.Instance.AssignSourcesAtRuntime(soundTypes, localSources);
        UIManager.Instance.AssignMainSceneRefsAtRuntime(localMainSceneRefs);
    }
}
