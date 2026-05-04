using UnityEngine;

public class PlayerIdleState : PlayerStates
{
    public PlayerIdleState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        // Enter Idle...
    }

    public override void Update()
    {
        PlayerController playerController = stateController.PlayerController;

        if (playerController.MoveInput != Vector2.zero)
            stateController.ChangedState(stateController.moveState);
    }

    public override void Exit()
    {
        // Exit Idle...
    }
}
