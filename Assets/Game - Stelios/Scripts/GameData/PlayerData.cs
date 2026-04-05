using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player")]
public class PlayerData : GameData
{
    [SerializeField] private string displayName;
    [SerializeField] private int maxLives;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    public string DisplayName => displayName;
    public int MaxLives => maxLives;
    public float Speed => speed; 
    public int Damage => damage;
}
