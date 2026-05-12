using UnityEngine;

public class PlayerMoveState : PlayerStates
{
    public PlayerMoveState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.AnimationPlayer.Play(stateController.MoveClip);
    }

    public override void Update()
    {
        PlayerController playerController = stateController.PlayerController;

        playerController.HandlePlayerRotation();
        playerController.ApplyMovement();

        if (playerController.MoveInput.sqrMagnitude < 0.01f)
        {
            stateController.ChangeState(stateController.idleState);
        }
    }

    public override void Exit()
    {
        // Exit Move...
    }
}
