using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreEventsSO", menuName = "ScoreEvent/Event")]
public class ScoreEventsSO : ScriptableObject
{
    public event Action OnScoreChanged;
    public event Action OnHighScoreChanged;

    public void RaiseScoreChanged() => OnScoreChanged?.Invoke();
    public void RaiseHighScoreChanged() => OnHighScoreChanged?.Invoke();
}
