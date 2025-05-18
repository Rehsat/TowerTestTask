using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using Game.Factories;
using Game.Factories.Figures;
using Game.Infrastructure;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.Configs;
using Game.Infrastructure.StateMachine.GameStates;
using Game.Services.Cameras;
using Game.Services.Canvases;
using Game.Services.DragAndDrop;
using Game.Services.FiguresCollections;
using Game.Services.Input;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private CoroutineStarter _coroutineStarter;
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private CanvasLayersService _canvasLayersService;

        public override void InstallBindings()
        {
            InstallConfig(_config);
            InstallInfrastructure();
            InstallFactories();
            InstallServices();
            InstallStateMachine();
        }

        private void InstallConfig(IGameConfig gameConfig)
        {
            Container.Bind<IPrefabsContainer>().FromInstance(gameConfig.PrefabsContainer).AsSingle();
            Container.Bind<IFigureSpriteByColorContainer>().FromInstance(gameConfig.FigureSpriteByColorContainer).AsSingle();
            Container.Bind<IFigureListConfigById>().FromInstance(gameConfig.StartFigureListConfigById).AsSingle();
        }
        
        private void InstallInfrastructure()
        {
            Container.Bind<ICoroutineStarter>().To<CoroutineStarter>().FromInstance(_coroutineStarter).AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().FromNew().AsSingle();
        }

        private void InstallFactories()
        {
            Container.Bind<IFactory<Canvas>>().To<CanvasFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<FiguresScrollView>>().To<FigureScrollViewFactory>().FromNew().AsSingle();

            Container.Bind<IFactory<FigureConfig, FigureData>>().To<FigureDataFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<FigureData, FigureUI>>().To<FigureUiViewFactory>().FromNew().AsSingle();
        }

// TODO: Поофиксит разрешение
        private void InstallServices()
        {
            Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().FromInstance(_cameraService).AsSingle();
            Container.Bind<ICanvasLayersService>().To<CanvasLayersService>().FromInstance(_canvasLayersService).AsSingle();
            Container.Bind<IDragService>().To<DragAndDropService>().FromNew().AsSingle();
            Container.Bind<IFiguresListsContainerService>().To<FiguresListsContainerService>().FromNew().AsSingle();
        }

        private void InstallStateMachine()
        {
            Container.Bind<IGameState>().To<BootstrapGameState>().FromNew().AsSingle();
            Container.Bind<IGameState>().To<TowerBuildState>().FromNew().AsSingle();

            Container.Bind<GameStateMachine>().FromNew().AsSingle();
        }
    }
}