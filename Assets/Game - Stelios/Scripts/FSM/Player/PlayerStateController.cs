using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerStates currentState;

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerController playerController;
    #endregion

    public PlayerController PlayerController => playerController;

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
    }

    private void Start()
    {
        ChangedState(idleState);
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangedState(PlayerStates newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
