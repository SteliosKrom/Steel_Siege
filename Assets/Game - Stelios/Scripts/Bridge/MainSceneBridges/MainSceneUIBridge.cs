using UnityEngine;

[System.Serializable]
public class MainSceneUIRefs
{
    public GameObject gameOverPanel;
    public GameObject player1WinsPanel;
    public GameObject player2WinsPanel;
    public GameObject drawPanel;
}

public class MainSceneUIBridge : MonoBehaviour
{
    [SerializeField] private MainSceneUIRefs localMainSceneUIRefs;

    private void Start()
    {
        UIManager.Instance.AssignMainSceneUIRefsAtRuntime(localMainSceneUIRefs);
    }
}
