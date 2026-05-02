using NUnit.Framework;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int highscore;
    public List<int> leaderboardScores;
    public List<string> leaderboardNames;
}
