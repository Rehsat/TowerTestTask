using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using Game.Core.UI;
using Game.Services.Canvases;
using Game.Services.DragAndDrop;
using Game.Services.FiguresCollections;
using Infrastructure.StateMachine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.StateMachine.GameStates
{
    public class TowerBuildState : IGameState
    {
        private readonly IDragDataHandleService _dragDataHandleService;
        private readonly IFactory<FigureData, FigureUI> _figureUIFactory;
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IFiguresListsContainerService _figuresListsContainerService;
        private readonly ICanvasLayersService _canvasLayersService;

        private FiguresScrollView _figuresScrollView;
        private FiguresScrollPresenter _figuresScrollPresenter;
        private CompositeDisposable _compositeDisposable;

        private bool _wasInitialized;
        public TowerBuildState(
            IDragDataHandleService dragDataHandleService,
            IFiguresListsContainerService figuresListsContainerService,
            ICanvasLayersService canvasLayersService,
            IFactory<FigureData, FigureUI> figureUIFactory,
            IFactory<FiguresScrollView> figureScrollViewFactory
            )
        {
            _dragDataHandleService = dragDataHandleService;
            _figureUIFactory = figureUIFactory;
            _figureScrollViewFactory = figureScrollViewFactory;
            _figuresListsContainerService = figuresListsContainerService;
            _canvasLayersService = canvasLayersService;
        }
        public void Enter()
        {
            Debug.LogError(123);
            if(_wasInitialized == false)
                Initialize();

            _compositeDisposable = new CompositeDisposable();
            _figuresScrollPresenter.OnNewFigureData
                .SubscribeWithSkip(_dragDataHandleService.HandleDragData)
                .AddTo(_compositeDisposable);
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
            
           _figuresScrollPresenter = new FiguresScrollPresenter(
               listOfFigures,
               _figureUIFactory,
               _figuresScrollView
               );
        }

        public void Exit()
        {
            if(_wasInitialized == false) return;
            
            _figuresScrollView.gameObject.SetActive(false);
            _compositeDisposable?.Dispose();
        }

        public void SetStateMachine(GameStateMachine stateMachine)
        {
        }
    }
}