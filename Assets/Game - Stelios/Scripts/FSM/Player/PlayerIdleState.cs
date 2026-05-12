using UnityEngine;

public class PlayerIdleState : PlayerStates
{
    public PlayerIdleState(PlayerStateController stateController) : base(stateController) { }

    public override void Enter()
    {
        stateController.AnimationPlayer.Stop();
    }

    public override void Update()
    {
        PlayerController playerController = stateController.PlayerController;

        if (playerController.MoveInput.sqrMagnitude > 0.01f)
        {
            stateController.ChangeState(stateController.moveState);
        }

    }

    public override void Exit()
    {
        // Exit Idle...
    }
}
