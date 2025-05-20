using Game.Core.BlackHole;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Core.Figures.Tower;
using Game.Core.Figures.UI;
using Game.Core.Figures.View;
using Game.Core.Figures.View.UI;
using Game.Factories;
using Game.Factories.Figures;
using Game.Infrastructure;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.Configs;
using Game.Infrastructure.CurrentLevelData;
using Game.Infrastructure.StateMachine.GameStates;
using Game.Services.Cameras;
using Game.Services.Canvases;
using Game.Services.DragAndDrop;
using Game.Services.FiguresCollections;
using Game.Services.Input;
using Game.Services.Localization;
using Game.Services.LogService;
using Game.Services.OutOfScreenCheck;
using Game.Services.RaycastService;
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
        [SerializeField] private CanvasLayersProvider canvasLayersProvider;
        [SerializeField] private PrefabsTransformContainer _prefabsTransformContainer;
        public override void InstallBindings()
        {
            InstallConfig(_config);
            InstallInfrastructure();
            InstallFactories();
            InstallServices();
            InstallStateMachine();
        }

        // В теории есть возможность заменить инстансы другими классами с тем же интерфейсом, тем самым поменять способ конфигураци.
        // Даже с сервера подгружать можно, если соответствующий класс создать, так что надеюсь выполнил требование 3 правильно
        private void InstallConfig(IGameConfig gameConfig)
        {
            Container.Bind<IPrefabsProvider>().FromInstance(gameConfig.PrefabsProvider).AsSingle();
            Container.Bind<IFigureSpriteByColorContainer>().FromInstance(gameConfig.FigureSpriteByColorContainer).AsSingle();
            Container.Bind<IFigureListConfigById>().FromInstance(gameConfig.StartFigureListConfigById).AsSingle();
        }
        
        private void InstallInfrastructure()
        {
            Container.Bind<IPrefabsTransformContainer>().FromInstance(_prefabsTransformContainer).AsSingle();
            Container.Bind<ICoroutineStarter>().To<CoroutineStarter>().FromInstance(_coroutineStarter).AsSingle();
            Container.Bind<ICurrentLevelDataProvider>().To<CurrentLevelDataProvider>().FromNew().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().FromNew().AsSingle();
        }

        private void InstallFactories()
        {
            Container.Bind<IFactory<Canvas>>().To<CanvasFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<FiguresScrollView>>().To<FigureScrollViewFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<TowerView>>().To<TowerViewFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<BlackHoleView>>().To<BlackHoleFactory>().FromNew().AsSingle();

            Container.Bind<IFactory<FigureConfig, FigureData>>().To<FigureDataFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<FigureData, FigureUI>>().To<FigureUiViewFactory>().FromNew().AsSingle();
            Container.Bind<IFactory<FigureData, FigureSpriteView>>().To<FigureSpriteViewFactory>().FromNew().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<ICanvasLayersProvider>().To<CanvasLayersProvider>().FromInstance(canvasLayersProvider).AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().FromInstance(_cameraService).AsSingle();
            Container.Bind<IRaycastService>().To<RaycastService>().FromNew().AsSingle();
            Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
            Container.Bind<IDragService>().To<DragAndDropService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<DragDataHandleService>().FromNew().AsSingle();
            Container.Bind<IFiguresListsContainerService>().To<FiguresListsContainerService>().FromNew().AsSingle();
            Container.Bind<IOutOfScreenCheckService>().To<OutOfScreenCheckService>().FromNew().AsSingle();
            Container.Bind<ILocalizationService>().To<MockLocalizationService>().FromNew().AsSingle();
            Container.Bind<ILogService>().To<LogService>().FromNew().AsSingle();
            
            Container.Bind<IInteractService>().To<InteractService>().FromNew().AsSingle().NonLazy();
        }

        private void InstallStateMachine()
        {
            Container.Bind<IGameState>().To<BootstrapGameState>().FromNew().AsSingle();
            Container.Bind<IGameState>().To<TowerBuildState>().FromNew().AsSingle();

            Container.Bind<GameStateMachine>().FromNew().AsSingle();
        }
    }
}