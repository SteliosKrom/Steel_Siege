using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Game Events/Events")]
public class GameEvents : ScriptableObject
{
    public UnityEvent OnGameOver;
    public UnityEvent OnP1Win;
    public UnityEvent OnP2Win;
    public UnityEvent OnDraw;

    public void RaiseGameOver()
    {
        OnGameOver?.Invoke();
    }

    public void RaiseP1Win()
    {
        OnP1Win?.Invoke();
    }

    public void RaiseP2Win()
    {
        OnP2Win?.Invoke();
    }

    public void RaiseDraw()
    {
        OnDraw?.Invoke();
    }
}
