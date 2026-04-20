using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventsSO", menuName = "Game Event/Event")]
public class GameEventsSO : ScriptableObject
{
    public event Action OnGameOver;
    public event Action OnP1Win;
    public event Action OnP2Win;
    public event Action OnDraw;
    public event Action OnPVPLoad;
    public event Action OnPVELoad;

    public void RaiseGameOver() => OnGameOver?.Invoke();
    public void RaiseP1Win() => OnP1Win?.Invoke();
    public void RaiseP2Win() => OnP2Win?.Invoke();
    public void RaiseDraw() => OnDraw?.Invoke();
    public void RaisePVPLoad() => OnPVPLoad?.Invoke();
    public void RaisePVELoad() => OnPVELoad?.Invoke();
}
