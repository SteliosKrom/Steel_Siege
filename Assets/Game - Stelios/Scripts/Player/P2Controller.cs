using System.Collections;
using UnityEngine;

public class P2Controller : MonoBehaviour
{
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
        Vector2 targetPos = playerRb.position + movementDirection.normalized * playerData.MoveSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(targetPos);
    }

    public void InputForPlayerMovement()
    {
        // P2 Movement...
        movementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection += Vector2.up;
            playerRb.SetRotation(0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection += Vector2.down;
            playerRb.SetRotation(180f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection += Vector2.left;
            playerRb.SetRotation(90f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += Vector2.right;
            playerRb.SetRotation(-90f);
        }
    }

    public void InputForPlayerShooting()
    {
        // P2 Shooting...
        if (Input.GetKeyDown(KeyCode.RightControl) && canShoot)
        {
            ShootBullet();
            StartCoroutine(ShootDelay());
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetObject("Bullet");
        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = shootingPoint.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootingPoint.up * playerData.BulletSpeed;
    }

    public IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
