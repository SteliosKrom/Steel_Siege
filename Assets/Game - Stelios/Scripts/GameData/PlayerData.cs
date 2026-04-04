using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player")]
public class PlayerData : GameData
{
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private Sprite[] animationFrame;
}
