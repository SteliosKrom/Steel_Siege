using UnityEngine;

public class PlayerMoveState : PlayerStates
{
    public override void Enter(PlayerStateController stateController)
    {
        // Enter Move...
    }

    public override void Update(PlayerStateController stateController)
    {
        PlayerController playerController = stateController.PlayerController;

        playerController.HandlePlayerRotation();
        playerController.ApplyMovement();

        if (playerController.MoveInput == Vector2.zero)
            stateController.ChangedState(stateController.idleState);
    }

    public override void Exit(PlayerStateController player)
    {
        // Exit Move...
    }
}
