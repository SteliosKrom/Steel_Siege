using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player")]
public class PlayerData : GameData
{
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;

    public int MaxLives => maxLives;
    public float MoveSpeed => moveSpeed;
    public float BulletSpeed => bulletSpeed;
    public int Damage => damage;
}
