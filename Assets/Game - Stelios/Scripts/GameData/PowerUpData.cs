using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Game Data/PowerUp")]
public class PowerUpData : GameData
{
    [SerializeField] private enum PowerUpType { Health, Speed}
    [SerializeField] private PowerUpType type;
    [SerializeField] private float duration;
}
