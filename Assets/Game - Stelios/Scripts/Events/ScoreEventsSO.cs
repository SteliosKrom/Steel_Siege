using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreEventsSO", menuName = "ScoreEvent/Event")]
public class ScoreEventsSO : ScriptableObject
{
    public event Action OnScoreChanged;

    // Raise On Score Changed...
    public void RaiseScoreChanged() => OnScoreChanged?.Invoke();
}
