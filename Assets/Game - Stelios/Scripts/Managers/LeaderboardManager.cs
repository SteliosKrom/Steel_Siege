using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [SerializeField] private LeaderboardSceneRefs leaderboardRefs;

    [SerializeField] private List<int> bestScores;

    private bool inserted = false;

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

    public void ProcessLeaderboard()
    {
        TryInsertCurrentScore();
        KeepTopScoresOnly();
    }

    public void TryInsertCurrentScore()
    {
        for (int i = 0; i <= bestScores.Count - 1; i++)
        {
            if (ScoreManager.Instance.CurrentScore > bestScores[i])
            {
                bestScores.Insert(i, ScoreManager.Instance.CurrentScore);
                Debug.Log("After Insert: " + string.Join(",", bestScores));
                inserted = true;
                break;
            }
        }
    }

    public void KeepTopScoresOnly()
    {
        if (inserted && bestScores.Count > 5)
        {
            bestScores.RemoveAt(bestScores.Count - 1);
            Debug.Log("After Trim: " + string.Join(",", bestScores));
        }
    }

    public void RefreshLeaderboardUI()
    {
        for (int i = 0; i <= leaderboardRefs.bestScoresText.Length - 1; i++)
        {
            leaderboardRefs.bestScoresText[i].text = bestScores[i].ToString("000000");
        }
    }

    public void SetLeaderboardRefs(LeaderboardSceneRefs localLeaderboardRefs)
    {
        leaderboardRefs = localLeaderboardRefs;
    }
}
