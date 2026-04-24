using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player")]
public class PlayerData : ScriptableObject
{
    public enum PlayerID { P1, P2 }

    [SerializeField] private PlayerID playerID;
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;

    public PlayerID PlayerType => playerID;
    public int MaxLives => maxLives;
    public float MoveSpeed => moveSpeed;
    public float BulletSpeed => bulletSpeed;
    public int Damage => damage;
}
