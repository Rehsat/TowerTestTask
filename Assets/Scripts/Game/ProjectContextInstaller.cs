using Game.Infrastructure;
using Game.Infrastructure.StateMachine.GameStates;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineStarter _coroutineStarter;
        public override void InstallBindings()
        {
            Debug.LogError(123);
            InstallInfrastructure();
            InstallStateMachine();
        }

        private void InstallInfrastructure()
        {
            Container.Bind<ICoroutineStarter>().To<CoroutineStarter>().FromInstance(_coroutineStarter).AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().FromNew().AsSingle();
        }

        private void InstallStateMachine()
        {
            Container.Bind<IGameState>().To<BootstrapGameState>().FromNew().AsSingle();
            Container.Bind<IGameState>().To<SquaresTowerBuildState>().FromNew().AsSingle();

            Container.Bind<GameStateMachine>().FromNew().AsSingle();
        }
    }
}