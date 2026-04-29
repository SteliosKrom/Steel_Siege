using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class LeaderboardSceneRefs
{
    public TextMeshProUGUI[] bestScoresText;
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
    }
}
