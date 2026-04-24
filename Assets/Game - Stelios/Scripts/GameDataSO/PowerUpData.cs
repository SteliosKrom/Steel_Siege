using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "PowerUpData/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public enum Type { Lives, Stamina}

    [SerializeField] private Type powerUpType;
    [SerializeField] private int effectDuration;

    public Type PowerUpType => powerUpType;
    public int EffectDuration => effectDuration;
}
