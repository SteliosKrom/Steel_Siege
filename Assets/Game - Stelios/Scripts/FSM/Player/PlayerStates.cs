public abstract class PlayerStates
{
    public abstract void Enter(PlayerStateController stateContoller);
    public abstract void Update(PlayerStateController stateController);
    public abstract void Exit(PlayerStateController stateController);
}
