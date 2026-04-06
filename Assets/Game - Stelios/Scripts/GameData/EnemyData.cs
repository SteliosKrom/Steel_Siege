using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy")]
public class EnemyData : GameData
{
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int scoreValue;
} 
