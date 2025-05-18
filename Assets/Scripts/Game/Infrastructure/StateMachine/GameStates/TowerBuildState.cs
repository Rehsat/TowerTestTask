using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using Game.Core.UI;
using Game.Services.Canvases;
using Game.Services.DragAndDrop;
using Game.Services.FiguresCollections;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.StateMachine.GameStates
{
    public class TowerBuildState : IGameState
    {
        private readonly IFactory<FigureData, FigureUI> _figureUIFactory;
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IDragService _dragService;
        private readonly IFiguresListsContainerService _figuresListsContainerService;
        private readonly ICanvasLayersService _canvasLayersService;

        private FiguresScrollView _figuresScrollView;

        private bool _wasInitialized;
        public TowerBuildState(
            IDragService dragService,
            IFiguresListsContainerService figuresListsContainerService,
            ICanvasLayersService canvasLayersService,
            IFactory<FigureData, FigureUI> figureUIFactory,
            IFactory<FiguresScrollView> figureScrollViewFactory
            )
        {
            _figureUIFactory = figureUIFactory;
            _figureScrollViewFactory = figureScrollViewFactory;
            _dragService = dragService;
            _figuresListsContainerService = figuresListsContainerService;
            _canvasLayersService = canvasLayersService;
        }
        public void Enter()
        {
            Debug.LogError(123);
            if(_wasInitialized == false)
                Initialize();
        }

        private void Initialize()
        {
            InitializeScrollPresenter();
            _wasInitialized = true;
        }
        private void InitializeScrollPresenter()
        {
            var targetCanvas = _canvasLayersService.GetCanvasByLayer(CanvasLayer.FiguresScroll);
            var listOfFigures = _figuresListsContainerService.GetListOfFigures(FigureListContainerId.Scroll);
            
            _figuresScrollView = _figureScrollViewFactory.Create();
            _figuresScrollView.transform.parent = targetCanvas.transform;
            _figuresScrollView.transform.localPosition = Vector3.zero;
            
           var squaresScrollPresenter = new FiguresScrollPresenter(
               listOfFigures,
               _figureUIFactory,
               _figuresScrollView,
               _dragService
               );
        }

        public void Exit()
        {
            if(_wasInitialized == false) return;
            _figuresScrollView.gameObject.SetActive(false);
        }

        public void SetStateMachine(GameStateMachine stateMachine)
        {
        }
    }
}