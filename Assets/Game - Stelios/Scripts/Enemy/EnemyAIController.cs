using System.Collections;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private float changeDirectionDelay;
    private float changeDirectionDelayVariation;
    private float shootDelay;
    private float shootDelayVariation;

    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform shootingPoint;

    private Vector2 moveDirection;

    private void Start()
    {
        changeDirectionDelay = 1.5f;
        shootDelay = 1.5f;

        ChooseDirection();
        StartCoroutine(ChangeDirectionDelay());
        StartCoroutine(ShootDelay());
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") ||
            other.gameObject.CompareTag("Obstacle"))
            ChooseDirection();
    }

    public void ChooseDirection()
    {
        if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            return;

        float rand = Random.Range(1, 5);

        switch (rand)
        {
            case 1:
                moveDirection = Move(Vector2.down, -180f);
                break;
            case 2:
                moveDirection = Move(Vector2.up, 0f);
                break;
            case 3:
                moveDirection = Move(Vector2.right, -90f);
                break;
            case 4:
                moveDirection = Move(Vector2.left, -270f);
                break;
        }
    }

    public Vector2 Move(Vector2 dir, float rotation)
    {
        moveDirection = dir;
        enemyRb.SetRotation(rotation);
        return moveDirection;
    }

    public void ApplyMovement()
    {
        if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            return;

        Vector2 targetPos = enemyRb.position + moveDirection.normalized * enemyData.MoveSpeed * Time.fixedDeltaTime;
        enemyRb.MovePosition(targetPos);
    }

    public void ShootBullet()
    {
        if (GameManager.Instance.CurrentGameState == GameState.GameOver)
            return;

        GameObject bullet = ObjectPoolManager.Instance.GetObject("Bullet");
        bullet.transform.position = shootingPoint.transform.position;
        bullet.transform.rotation = shootingPoint.transform.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootingPoint.up * enemyData.BulletSpeed;
        AudioManager.Instance.PlaySFX(AudioManager.SoundType.Shoot);
    }

    public IEnumerator ChangeDirectionDelay()
    {
        while (true)
        {
            changeDirectionDelayVariation = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(changeDirectionDelay + changeDirectionDelayVariation);
            ChooseDirection();
        }
    }

    public IEnumerator ShootDelay()
    {
        while (true)
        {
            shootDelayVariation = Random.Range(0.3f, 0.7f);
            yield return new WaitForSeconds(shootDelay + shootDelayVariation);
            ShootBullet();
        }
    }
}
