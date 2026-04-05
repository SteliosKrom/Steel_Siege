using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private PlayerData playerData;
    private Vector2 movementDirection;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0f)
        {
            vertical = 0f;
        }

        movementDirection = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        if (movementDirection.x > 0) playerRb.SetRotation(-90f);
        else if (movementDirection.x < 0) playerRb.SetRotation(90f);
        else if (movementDirection.y > 0) playerRb.SetRotation(0f);
        else if (movementDirection.y < 0) playerRb.SetRotation(180f);

        playerRb.linearVelocity = movementDirection * playerData.Speed;
    }
}
