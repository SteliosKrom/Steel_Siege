public abstract class PlayerStates
{
    protected PlayerStateController stateController;

    public PlayerStates(PlayerStateController stateController)
    {
        this.stateController = stateController;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
