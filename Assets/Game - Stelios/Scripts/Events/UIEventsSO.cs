using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEventsSO", menuName = "UIEvent/Event")]
public class UIEventsSO : ScriptableObject
{
    public PlayerHealth playerHealth;

    public event Action OnHideRedLives;
    public event Action OnEnablePVPLives;
    public event Action OnEnablePVELives;
    public event Action<PlayerData.PlayerID, int> OnDecreaseLivesUI;
    public event Action<PlayerData.PlayerID, int> OnIncreaseLivesUI;

    public event Action OnShowPVEScore;
    public event Action OnScoreUIChanged;
    public event Action OnHighScoreUIChanged;

    public event Action OnInsertCoin;

    public event Action OnEnableGameModes;
    public event Action OnEnableWavesUI;
    public event Action OnDisableWavesUI;

    public event Action OnPVPStay;
    public event Action OnPVPExit;
    public event Action OnPVEStay;
    public event Action OnPVEExit;

    public event Action OnPVPSelected;
    public event Action OnPVESelected;

    public void RaiseInsertCoin() => OnInsertCoin?.Invoke();
    public void RaiseEnableGameModes() => OnEnableGameModes?.Invoke();
    public void RaiseEnableWavesUI() => OnEnableWavesUI?.Invoke();
    public void RaiseDisableWavesUI() => OnDisableWavesUI?.Invoke();

    // Raise Lives...
    public void RaiseHideRedLives() => OnHideRedLives?.Invoke();
    public void RaiseEnablePVPLives() => OnEnablePVPLives?.Invoke();
    public void RaiseEnablePVELives() => OnEnablePVELives?.Invoke();
    public void RaiseDecreaseLivesUI(PlayerData.PlayerID playerID, int currentLives) => OnDecreaseLivesUI?.Invoke(playerID, currentLives);
    public void RaiseIncreaseLivesUI(PlayerData.PlayerID playerID, int currentLives) => OnIncreaseLivesUI?.Invoke(playerID, currentLives);

    // Raise Scores...
    public void RaiseShowPVEScore() => OnShowPVEScore?.Invoke();
    public void RaiseScoreUIChanged() => OnScoreUIChanged?.Invoke();
    public void RaiseHighScoreUIChanged() => OnHighScoreUIChanged?.Invoke();

    // Raise Modes Stay & Exit...
    public void RaisePVPStay() => OnPVPStay?.Invoke();
    public void RaisePVPExit() => OnPVPExit?.Invoke();
    public void RaisePVEStay() => OnPVEStay?.Invoke();
    public void RaisePVEExit() => OnPVEExit?.Invoke();

    // Raise Modes Selection...
    public void RaisePVPSelected() => OnPVPSelected?.Invoke();
    public void RaisePVESelected() => OnPVESelected?.Invoke();
}
