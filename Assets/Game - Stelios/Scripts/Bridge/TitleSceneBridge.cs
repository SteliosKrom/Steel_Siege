using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class TitleSceneRefs
{
    public GameObject modesMenu;
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
        uiEvents.RaiseHighScoreUIChanged();
        ResetCreditsUI();
    }

    public void ResetCreditsUI()
    {
        titleRefs.insertCoinText.enabled = true;
        titleRefs.creditCoinText.text = "00";
        UIManager.Instance.CreditCounter = 0;
    }
}
