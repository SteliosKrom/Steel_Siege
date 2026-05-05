using TMPro;
using UnityEngine;

[System.Serializable]
public class TitleSceneRefs
{
    public GameObject modesMenu;
    public GameObject debugOverlay;
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI activeObjectsText;
    public TextMeshProUGUI activeRigidBodiesText;
    public TextMeshProUGUI numberOfDrawCallsText;
    public TextMeshProUGUI ramUsageText;
    public TextMeshProUGUI insertCoinText;
    public TextMeshProUGUI pressStartText;
    public TextMeshProUGUI creditCoinText;
    public TextMeshProUGUI PVPText;
    public TextMeshProUGUI PVEText;
    public TextMeshProUGUI PVPSelectionArrow;
    public TextMeshProUGUI PVESelectionArrow;
    public TextMeshProUGUI highscoreText;
}

public class TitleSceneBridge : MonoBehaviour
{
    #region LOCAL REFERENCES
    [Header("LOCAL REFERENCES")]
    [SerializeField] private TitleSceneRefs titleRefs;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private UIEventsSO uiEvents;
    #endregion

    private void Start()
    {
        UIManager.Instance.SetTitleUI(titleRefs);
        UIManager.Instance.SetDebugOverlay(titleRefs.debugOverlay);
        ResetCreditsUI();

        UIManager.Instance.SetUIReady(true);
    }

    public void ResetCreditsUI()
    {
        titleRefs.insertCoinText.enabled = true;
        titleRefs.creditCoinText.text = "00";
        UIManager.Instance.CreditCounter = 0;
    }
}
