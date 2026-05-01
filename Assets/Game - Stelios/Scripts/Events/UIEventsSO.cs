using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "UIEventsSO", menuName = "UIEvent/Event")]
public class UIEventsSO : ScriptableObject
{
    public event Action OnHideRedLives;
    public event Action OnEnablePVPLives;
    public event Action OnEnablePVELives;
    public event Action<PlayerData.PlayerID, int> OnDecreaseLivesUI;
    public event Action<PlayerData.PlayerID, int> OnIncreaseLivesUI;

    public event Action OnShowPVEScore;
    public event Action OnScoreUIChanged;
    public event Action OnHighScoreUIChanged;
    public event Action OnLeaderboardScoresAndNamesUIChanged;

    public event Action OnInsertCoin;
    public event Action OnShowEnterYourNamePanel;

    public event Action OnEnableGameModes;
    public event Action OnEnableWavesUI;
    public event Action OnDisableWavesUI;

    public event Action OnPVPStay;
    public event Action OnPVPExit;
    public event Action OnPVEStay;
    public event Action OnPVEExit;

    public event Action OnFirstLetterStay;
    public event Action OnFirstLetterExit;
    public event Action OnSecondLetterStay;
    public event Action OnSecondLetterExit;
    public event Action OnThirdLetterStay;
    public event Action OnThirdLetterExit;

    public event Action OnPVPSelected;
    public event Action OnPVESelected;

    public void RaiseInsertCoin() => OnInsertCoin?.Invoke();
    public void RaiseShowEnterYourNamePanel() => OnShowEnterYourNamePanel?.Invoke();
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
    public void RaiseLeaderboardScoresAndNamesUIChanged() => OnLeaderboardScoresAndNamesUIChanged?.Invoke();

    // Raise Modes Stay & Exit...
    public void RaisePVPStay() => OnPVPStay?.Invoke();
    public void RaisePVPExit() => OnPVPExit?.Invoke();
    public void RaisePVEStay() => OnPVEStay?.Invoke();
    public void RaisePVEExit() => OnPVEExit?.Invoke();

    // Raise Letters Stay & Exit...
    public void RaiseFirstLetterStay() => OnFirstLetterStay?.Invoke();
    public void RaiseFirstLetterExit() => OnFirstLetterExit?.Invoke();
    public void RaiseSecondLetterStay() => OnSecondLetterStay?.Invoke();
    public void RaiseSecondLetterExit() => OnSecondLetterExit?.Invoke();
    public void RaiseThirdLetterStay() => OnThirdLetterStay?.Invoke();
    public void RaiseThirdLetterExit() => OnThirdLetterExit?.Invoke();


    // Raise Modes Selection...
    public void RaisePVPSelected() => OnPVPSelected?.Invoke();
    public void RaisePVESelected() => OnPVESelected?.Invoke();
}
