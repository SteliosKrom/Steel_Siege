using System.Collections;
using UnityEngine;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private UIEventsSO uiEvents;
    [SerializeField] private ScoreEventsSO scoreEvents;

    [SerializeField] private bool resetSaveForTesting;

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (resetSaveForTesting)
        {
            File.Delete(savePath);
        }

        LoadFromFile();
    }

    public void SaveAll()
    {
        SaveData data = LoadOrCreate();

        data.highscore = ScoreManager.Instance.CurrentHighScore;
        data.leaderboardScores = new List<int>(LeaderboardManager.Instance.BestScores);
        data.leaderboardNames = new List<string>(LeaderboardManager.Instance.BestScoreNames);

        File.WriteAllText(savePath, JsonUtility.ToJson(data));
    }

    public SaveData LoadOrCreate()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return new SaveData();
    }

    public void LoadFromFile()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            ScoreManager.Instance.CurrentHighScore = data.highscore;

            UIManager.Instance.TitleRefs.highscoreText.text = ScoreManager.Instance.CurrentHighScore.ToString("000000");

            LeaderboardManager.Instance.BestScores = new List<int>(data.leaderboardScores);
            LeaderboardManager.Instance.BestScoreNames = new List<string>(data.leaderboardNames);

            while (LeaderboardManager.Instance.BestScores.Count < 5)
            {
                LeaderboardManager.Instance.BestScores.Add(0);
                LeaderboardManager.Instance.BestScoreNames.Add("---");
            }
        }
        else
        {
            ScoreManager.Instance.CurrentHighScore = 0;
        }
    }
}
