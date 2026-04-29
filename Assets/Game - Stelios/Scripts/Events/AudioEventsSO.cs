using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioEventsSO", menuName = "AudioEvent/Event")]
public class AudioEventsSO : ScriptableObject
{
    public event Action<AudioManager.SoundType> OnShoot;
    public event Action<AudioManager.SoundType> OnHurt;
    public event Action<AudioManager.SoundType> OnExplosion;
    public event Action<AudioManager.SoundType> OnHitWall;
    public event Action<AudioManager.SoundType> OnGainPowerUp;
    public event Action<AudioManager.SoundType> OnSpawnPowerUp;
    public event Action<AudioManager.SoundType> OnGameOver;

    public void RaiseShoot() => OnShoot?.Invoke(AudioManager.SoundType.Shoot);
    public void RaiseHurt() => OnHurt?.Invoke(AudioManager.SoundType.Hurt);
    public void RaiseExplosion() => OnExplosion?.Invoke(AudioManager.SoundType.Explosion);
    public void RaiseHitWall() => OnHitWall?.Invoke(AudioManager.SoundType.Hit);
    public void RaiseGainPowerUp() => OnGainPowerUp?.Invoke(AudioManager.SoundType.GainPowerUp);
    public void RaiseSpawnPowerUp() => OnSpawnPowerUp?.Invoke(AudioManager.SoundType.SpawnPowerUp);
    public void RaiseGameOver() => OnGameOver?.Invoke(AudioManager.SoundType.GameOver);
}
