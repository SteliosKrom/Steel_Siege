using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] private string bulletTag;

    private float reachThreshold = 0.1f;
    private float changeDirectionDelay;
    private float changeDirectionDelayVariation;
    private float shootDelay;
    private float shootDelayVariation;

    private NavMeshAgent navMeshAgent;
    private Transform player;

    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private MovementType movementType;

    private Vector2 moveDirection;
    private Vector2 lastPlayerPosition;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        changeDirectionDelay = 1.5f;
        shootDelay = 1.5f;
        moveDirection = Vector3.zero;

        if (movementType == MovementType.Pathfinding)
        {
            player = GameObject.FindGameObjectWithTag("Player1").transform;
            navMeshAgent.SetDestination(player.position);
            lastPlayerPosition = player.position;
        }
        else
        {
            ChooseDirection();
            StartCoroutine(ChangeDirectionDelay());
        }

        StartCoroutine(ShootDelay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        enemyRb.linearVelocity = Vector3.zero;
        enemyRb.angularVelocity = 0f;
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Random)
        {
            ApplyMovement();
        }
        else if (movementType == MovementType.Pathfinding)
        {
            float distanceToPlayer = Vector2.Distance(player.position, lastPlayerPosition);

            if (distanceToPlayer > reachThreshold)
            {
                navMeshAgent.SetDestination(player.position);
                lastPlayerPosition = player.position;
            }
            UpdatePathDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (movementType == MovementType.Random &&
            (other.gameObject.CompareTag("Wall") ||
            other.gameObject.CompareTag("Obstacle")))
        {
            ChooseDirection();
        }
    }

    public void UpdatePathDirection()
    {
        Vector2 direction = navMeshAgent.desiredVelocity.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                moveDirection = Vector2.right;
                enemyRb.SetRotation(-90f);
            }
            else
            {
                moveDirection = Vector2.left;
                enemyRb.SetRotation(-270f);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                moveDirection = Vector2.up;
                enemyRb.SetRotation(0f);
            }
            else
            {
                moveDirection = Vector2.down;
                enemyRb.SetRotation(-180f);
            }
        }   
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

        GameObject bullet = ObjectPoolManager.Instance.GetObject(bulletTag);
        bullet.transform.position = shootingPoint.transform.position;
        bullet.transform.rotation = shootingPoint.transform.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0f;
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
