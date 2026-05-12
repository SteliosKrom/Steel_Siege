using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerStates currentState;

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;

    [SerializeField] private AnimationPlayer animationPlayer;
    [SerializeField] private AnimationClipData moveClip;

    #region PROPERTIES
    public AnimationPlayer AnimationPlayer => animationPlayer;
    public AnimationClipData MoveClip => moveClip;
    #endregion

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
        ChangeState(idleState);
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeState(PlayerStates newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
