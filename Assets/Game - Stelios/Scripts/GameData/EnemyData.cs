using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int scoreValue;

    public int MaxLives => maxLives;
    public int Damage => damage;
    public int ScoreValue => scoreValue;
    public float MoveSpeed => moveSpeed;
    public float BulletSpeed => bulletSpeed;
} 
