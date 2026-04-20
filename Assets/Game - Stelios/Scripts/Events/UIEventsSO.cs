using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEventsSO", menuName = "UIEvent/Event")]
public class UIEventsSO : ScriptableObject
{
    public event Action OnHideRedLives;
    public event Action OnShowPVEScore;
    public event Action OnInsertCoin;
    public event Action OnEnableGameModes;
    public event Action OnPVPStay;
    public event Action OnPVPExit;
    public event Action OnPVEStay;
    public event Action OnPVEExit;
    public event Action OnPVPSelected;
    public event Action OnPVESelected;

    public void RaiseHideRedLives() => OnHideRedLives?.Invoke();
    public void RaiseShowPVEScore() => OnShowPVEScore?.Invoke();
    public void RaiseInsertCoin() => OnInsertCoin?.Invoke();
    public void RaiseEnableGameModes() => OnEnableGameModes?.Invoke();
    public void RaisePVPStay() => OnPVPStay?.Invoke();
    public void RaisePVPExit() => OnPVPExit?.Invoke();
    public void RaisePVEStay() => OnPVEStay?.Invoke();
    public void RaisePVEExit() => OnPVEExit?.Invoke();
    public void RaisePVPSelected() => OnPVPSelected?.Invoke();
    public void RaisePVESelected() => OnPVESelected?.Invoke();
}
