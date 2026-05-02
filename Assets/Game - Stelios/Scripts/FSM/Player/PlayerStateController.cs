using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerStates currentState;

    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerController playerController;
    #endregion

    public PlayerController PlayerController => playerController;

    private void Start()
    {
        ChangedState(idleState);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void ChangedState(PlayerStates newState)
    {
        if (currentState != null)
            currentState.Exit(this);

        currentState = newState;
        currentState.Enter(this);
    }
}
