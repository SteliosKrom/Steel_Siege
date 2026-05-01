using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [SerializeField] private LeaderboardSceneRefs leaderboardRefs;

    [SerializeField] private List<int> bestScores;
    [SerializeField] private List<string> bestScoreNames;

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private ScoreEventsSO scoreEvents;
    #endregion

    public List<int> BestScores { get => bestScores; set => bestScores = value; }
    public List<string> BestScoreNames { get => bestScoreNames; set => bestScoreNames = value; }
    public LeaderboardSceneRefs LeaderboardRefs => leaderboardRefs;

    private void Awake()
    {
        transform.SetParent(null);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InsertScore(int score, string name)
    {
        bool inserted = false;

        for (int i = 0; i < bestScores.Count; i++)
        {
            if (score > bestScores[i])
            {
                bestScores.Insert(i, score);
                bestScoreNames.Insert(i, name);
                inserted = true;
                break;
            }
        }

        if (!inserted)
        {
            bestScores.Add(score);
            bestScoreNames.Add(name);
        }

        if (bestScores.Count > 5)
        {
            bestScores.RemoveAt(bestScores.Count - 1);
            bestScoreNames.RemoveAt(bestScoreNames.Count - 1);
        }

        Debug.Log("Leaderboard: " + string.Join(",", bestScores));
    }

    public void SetLeaderboardRefs(LeaderboardSceneRefs localLeaderboardRefs)
    {
        leaderboardRefs = localLeaderboardRefs;
    }
}
