using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "PowerUpData/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public enum Type { Lives, Stamina}

    [SerializeField] private Type powerUpType;
    public Type PowerUpType => powerUpType;
}
