using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class LeaderboardSceneRefs
{
    public GameObject debugOverlay;

    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI activeObjectsText;
    public TextMeshProUGUI activeRigidBodiesText;
    public TextMeshProUGUI numberOfDrawCallsText;
    public TextMeshProUGUI ramUsageText;

    public TextMeshProUGUI[] bestScoresText;
    public TextMeshProUGUI[] bestScoreNamesText;
}

public class LeaderboardSceneBridge : MonoBehaviour
{
    #region LOCAL REFERENENCES
    [Header("LOCAL REFERENCES")]
    [SerializeField] private LeaderboardSceneRefs leaderboardRefs;
    #endregion

    private void Awake()
    {
        LeaderboardManager.Instance.SetLeaderboardRefs(leaderboardRefs);
        UIManager.Instance.SetDebugOverlay(leaderboardRefs.debugOverlay);

        UIManager.Instance.SetUIReady(true);
    }
}
