using UnityEngine;

public class PlayerIdleState : PlayerStates
{
    public override void Enter(PlayerStateController stateController)
    {
        // Enter Idle...
    }

    public override void Update(PlayerStateController stateController)
    {
        PlayerController playerController = stateController.PlayerController;

        if (playerController.MoveInput != Vector2.zero)
            stateController.ChangedState(stateController.moveState);
    }

    public override void Exit(PlayerStateController stateController)
    {
        // Exit Idle...
    }
}
