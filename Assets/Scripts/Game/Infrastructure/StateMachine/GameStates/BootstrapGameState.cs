using Game.Infrastructure.AssetsManagement;
using Game.Services.Canvases;
using Game.Services.Save;
using Infrastructure.StateMachine;
using UniRx;
using UnityEngine;

namespace Game.Infrastructure.StateMachine.GameStates
{
    public class BootstrapGameState : IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ICanvasLayersProvider _canvasLayersProvider;
        private readonly IPrefabsTransformContainer _prefabsTransformContainer;
        private readonly ISaveService _saveService;

        private GameStateMachine _stateMachine;

        private const string MAIN_SCENE_NAME = "MainScene";

        public BootstrapGameState(ISceneLoader sceneLoader,
            ICanvasLayersProvider canvasLayersProvider,
            IPrefabsTransformContainer prefabsTransformContainer,
            ISaveService saveService)
        {
            _sceneLoader = sceneLoader;
            _canvasLayersProvider = canvasLayersProvider;
            _prefabsTransformContainer = prefabsTransformContainer;
            _saveService = saveService;
        }
        public void SetStateMachine(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void Enter()
        {
            var scrollCanvas = _canvasLayersProvider.GetCanvasByLayer(CanvasLayer.FiguresScroll);
            _prefabsTransformContainer.AddTransform(Prefab.FiguresScroll, scrollCanvas.transform);
            
            _saveService.Load(); // под загрузку можно выделить отдельный стейт при нееобходимости
            _sceneLoader.LoadScene(MAIN_SCENE_NAME, OnEnterMainScene);
        }

        private void OnEnterMainScene()
        {
            _stateMachine.EnterState<TowerBuildState>();
        }
        
        public void Exit()
        {
        }
    }
}