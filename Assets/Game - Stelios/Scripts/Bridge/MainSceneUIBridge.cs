using UnityEngine;

public class MainSceneUIBridge : MonoBehaviour
{
    [SerializeField] private GameObject localGameOverPanel;

    private void Start()
    {
        UIManager.Instance.GameOverPanel = localGameOverPanel;
    }

    private void OnDestroy()
    {
        UIManager.Instance.GameOverPanel = null;
    }
}
