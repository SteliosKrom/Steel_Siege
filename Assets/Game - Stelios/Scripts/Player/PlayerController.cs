using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode shootKey;

    private float shootDelay = 0.15f;
    private bool canShoot = true;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Transform shootingPoint;
    private Vector2 movementDirection;

    private void Update()
    {
        InputForPlayerMovement();
        InputForPlayerShooting();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void InputForPlayerMovement()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        // Player Movement. Change to Arcade Machine inputs later...
        movementDirection = Vector2.zero;

        if (Input.GetKey(upKey)) Move(Vector2.up, 0f);
        else if (Input.GetKey(downKey)) Move(Vector2.down, 180f);
        else if (Input.GetKey(leftKey)) Move(Vector2.left, 90f);
        else if (Input.GetKey(rightKey)) Move(Vector2.right, -90f);
    }

    public void InputForPlayerShooting()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        // Player Shooting. Change to Arcade Machine inputs later...
        if (Input.GetKeyDown(shootKey) && canShoot)
        {
            ShootBullet();
            StartCoroutine(ShootDelay());
        }
    }

    public void Move(Vector2 dir, float rotation)
    {
        movementDirection += dir;
        playerRb.SetRotation(rotation);
    }

    public void ApplyMovement()
    {
        Vector2 targetPos = playerRb.position + movementDirection.normalized * playerData.MoveSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(targetPos);
    }

    public void ShootBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetObject("Bullet");
        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = shootingPoint.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootingPoint.up * playerData.BulletSpeed;
        AudioManager.Instance.PlaySFX(AudioManager.SoundType.Shoot);
    }

    public IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
