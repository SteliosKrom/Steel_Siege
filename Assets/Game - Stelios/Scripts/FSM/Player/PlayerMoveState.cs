using UnityEngine;

public class PlayerMoveState : PlayerStates
{
    public PlayerMoveState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        // Enter Move...
    }

    public override void Update()
    {
        PlayerController playerController = stateController.PlayerController;

        playerController.HandlePlayerRotation();
        playerController.ApplyMovement();

        if (playerController.MoveInput == Vector2.zero)
            stateController.ChangedState(stateController.idleState);
    }

    public override void Exit()
    {
        // Exit Move...
    }
}
