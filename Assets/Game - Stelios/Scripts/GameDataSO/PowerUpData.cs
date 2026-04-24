using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "PowerUpData/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public enum Type { Lives, Stamina}

    [SerializeField] private Type type;
    [SerializeField] private int effectDuration;

    public Type PowerUpType => type;
    public int EffectDuration => effectDuration;
}
