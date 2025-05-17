namespace Infrastructure.StateMachine
{
    public interface IGameState : IState
    {
        public void SetStateMachine(GameStateMachine stateMachine);
    }
}