using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private float shootDelay = 0.75f;
    private bool canShoot = true;

    [SerializeField] private string bulletTag;

    #region INPUT 
    private PlayerControls playerControls;
    private Vector2 moveInput;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerStamina playerStamina;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Transform shootingPoint;
    private Vector2 moveDirection;
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        if (playerData.PlayerType == PlayerData.PlayerID.P1)
        {
            playerControls.P1.Move.performed += OnMove;
            playerControls.P1.Move.canceled += OnMove;

            playerControls.P1.Shoot.performed += OnShoot;
        }
        else
        {
            playerControls.P2.Move.performed += OnMove;
            playerControls.P2.Move.canceled += OnMove;

            playerControls.P2.Shoot.performed += OnShoot;
        }
    }

    private void OnDisable()
    {
        if (playerData.PlayerType == PlayerData.PlayerID.P1)
        {
            playerControls.P1.Move.performed -= OnMove;
            playerControls.P1.Move.canceled -= OnMove;

            playerControls.P1.Shoot.performed -= OnShoot;

        }
        else
        {
            playerControls.P2.Move.performed -= OnMove;
            playerControls.P2.Move.canceled -= OnMove;

            playerControls.P2.Shoot.performed -= OnShoot;
        }
        playerControls.Disable();
    }

    private void FixedUpdate()
    {
        HandlePlayerRotation();
        ApplyMovement();
    }

    public void OnMove(InputAction.CallbackContext cxt)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        // Change to Arcade Machine inputs later...
        moveInput = cxt.ReadValue<Vector2>();

        if (moveInput.x > 0)
            moveDirection = Vector2.right;
        else if (moveInput.x < 0)
            moveDirection = Vector2.left;
        else if (moveInput.y > 0)
            moveDirection = Vector2.up;
        else if (moveInput.y < 0)
            moveDirection = Vector2.down;
        else
            moveDirection = Vector2.zero;
    }

    public void OnShoot(InputAction.CallbackContext cxt)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        // Player Shooting. Change to Arcade Machine inputs later...
        if (canShoot)
        {
            ShootBullet();
            StartCoroutine(ShootDelay());
        }
    }

    public void HandlePlayerRotation()
    {
        if (moveDirection.y > 0) playerRb.SetRotation(0f);
        else if (moveDirection.y < 0) playerRb.SetRotation(180f);
        else if (moveDirection.x < 0) playerRb.SetRotation(90f);
        else if (moveDirection.x > 0) playerRb.SetRotation(-90f);
    }

    public void ApplyMovement()
    {
        Vector2 targetPos = playerRb.position + moveDirection.normalized * playerStamina.CurrentSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(targetPos);
    }

    public void ShootBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetObject(bulletTag);
        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = shootingPoint.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootingPoint.up * playerData.BulletSpeed;
        audioEvents.RaiseShoot();
    }

    public IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
