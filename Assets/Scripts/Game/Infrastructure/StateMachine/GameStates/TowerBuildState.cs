using System.Collections.Generic;
using Game.Core.Figures;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Core.Figures.Tower;
using Game.Core.Figures.UI;
using Game.Core.Figures.View;
using Game.Core.Figures.View.UI;
using Game.Core.UI;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.CurrentLevelData;
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
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteView;
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IFactory<TowerView> _towerViewFactory;
        private readonly IFiguresListsContainerService _figuresListsContainerService;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;

        private FiguresScrollView _figuresScrollView;
        private TowerView _towerView;

        private List<IDragFigureDataCreator> _dragFigureDataCreators;
        
        private CompositeDisposable _compositeDisposable;

        private bool _wasInitialized;
        public TowerBuildState(
            IDragDataHandleService dragDataHandleService,
            IFiguresListsContainerService figuresListsContainerService,
            ICurrentLevelDataProvider currentLevelDataProvider,
            IFactory<FigureConfig, FigureData> figureDataFactory,
            IFactory<FigureData, FigureUI> figureUIFactory,
            IFactory<FigureData, FigureSpriteView> figureSpriteView,
            IFactory<TowerView> towerViewFactory
            )
        {
            _dragDataHandleService = dragDataHandleService;
            _figureUIFactory = figureUIFactory;
            _figureSpriteView = figureSpriteView;
            _towerViewFactory = towerViewFactory;
            _figuresListsContainerService = figuresListsContainerService;
            _currentLevelDataProvider = currentLevelDataProvider;
            _figureDataFactory = figureDataFactory;
            _dragFigureDataCreators = new List<IDragFigureDataCreator>();
        }
        public void Enter()
        {
            if(_wasInitialized == false)
                Initialize();

            _compositeDisposable = new CompositeDisposable();
            _dragFigureDataCreators.ForEach(creator =>
            {
                creator.OnNewDragFigureData
                    .SubscribeWithSkip(_dragDataHandleService.HandleDragData)
                    .AddTo(_compositeDisposable);
            });
        }

        private void Initialize()
        {
            InitializeScrollPresenter();
            InitializeTowerPresenter();
            _wasInitialized = true;
        }
        private void InitializeScrollPresenter()
        {
            var listOfFigures = _figuresListsContainerService.GetListOfFigures(FigureListContainerId.Scroll);

            _figuresScrollView =
                _currentLevelDataProvider.CurrentLevelData.GetPrefabsComponent<FiguresScrollView>(Prefab.FiguresScroll);
            
           var figuresScrollPresenter = new FiguresScrollPresenter(
               listOfFigures,
               _figureUIFactory,
               _figuresScrollView,
               _figureDataFactory
               );
           _dragFigureDataCreators.Add(figuresScrollPresenter);
        }

        private void InitializeTowerPresenter()
        {
            _towerView = _towerViewFactory.Create();
            var listOfFigures = _figuresListsContainerService.GetListOfFigures(FigureListContainerId.Tower);
            
            var towerPresenter = new TowerPresenter(listOfFigures, _towerView, _figureSpriteView);
            _dragFigureDataCreators.Add(towerPresenter);
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