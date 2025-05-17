using Infrastructure.StateMachine;

namespace Game.Infrastructure.StateMachine.GameStates
{
    public class BootstrapGameState : IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private global::Infrastructure.StateMachine.GameStateMachine _stateMachine;

        private const string MAIN_SCENE_NAME = "MainScene";

        public BootstrapGameState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine; // в теории для этого метода можно выделить абстрактный класс, но мне кажется он осбо много не даст пользы, а лишнее наследование появится
        }
        public void Enter()
        {
            _sceneLoader.LoadScene(MAIN_SCENE_NAME, OnEnterMainScene);
        }

        private void OnEnterMainScene()
        {
            //_stateMachine.EnterState<>();
        }
        public void Exit()
        {
        }
    }
}