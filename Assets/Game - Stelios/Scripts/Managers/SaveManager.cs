using System.Collections;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private UIEventsSO uiEvents;

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
        LoadFromFile();
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highscore = ScoreManager.Instance.CurrentHighScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadFromFile()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            ScoreManager.Instance.CurrentHighScore = data.highscore;
        }
        else
        {
            ScoreManager.Instance.CurrentHighScore = 0;
        }
        uiEvents.RaiseHighScoreUIChanged();
    }
}
