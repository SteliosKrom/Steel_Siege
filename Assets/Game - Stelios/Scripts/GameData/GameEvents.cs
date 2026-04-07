using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Game Events/Events")]
public class GameEvents : ScriptableObject
{
    public UnityEvent OnGameOver;

    public void RaiseGameOver()
    {
        OnGameOver?.Invoke();
    }
}
