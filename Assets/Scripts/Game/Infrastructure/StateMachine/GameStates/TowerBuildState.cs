using System.Collections.Generic;
using Game.Core.BlackHole;
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
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IFactory<TowerView> _towerViewFactory;
        private readonly IFactory<IBlackHoleView> _blackHoleFactory;
        private readonly IFiguresListsContainerService _figuresListsContainerService;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;

        
        private List<IDragFigureDataCreator> _dragFigureDataCreators;
        private List<GameObject> _objectsToActivate;
        
        private CompositeDisposable _compositeDisposable;

        private bool _wasInitialized;
        public TowerBuildState(
            IDragDataHandleService dragDataHandleService,
            IFiguresListsContainerService figuresListsContainerService,
            ICurrentLevelDataProvider currentLevelDataProvider,
            IFactory<FigureConfig, FigureData> figureDataFactory,
            IFactory<FigureData, FigureUI> figureUIFactory,
            IFactory<FigureData, FigureSpriteView> figureSpriteViewFactory,
            IFactory<TowerView> towerViewFactory,
            IFactory<BlackHoleView> blackHoleFactory
            )
        {
            _dragDataHandleService = dragDataHandleService;
            _figureUIFactory = figureUIFactory;
            _figureSpriteViewFactory = figureSpriteViewFactory;
            _towerViewFactory = towerViewFactory;
            _blackHoleFactory = blackHoleFactory;
            _figuresListsContainerService = figuresListsContainerService;
            _currentLevelDataProvider = currentLevelDataProvider;
            _figureDataFactory = figureDataFactory;
            
            _dragFigureDataCreators = new List<IDragFigureDataCreator>();
            _objectsToActivate = new List<GameObject>();
        }
        public void Enter()
        {
            if(_wasInitialized == false)
                Initialize();

            _compositeDisposable = new CompositeDisposable();
            _objectsToActivate.ForEach(gameObjectToActivate => 
                gameObjectToActivate.gameObject.SetActive(true));
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
            InitializeBlackHole();
            _wasInitialized = true;
        }
        private void InitializeScrollPresenter()
        {
            var listOfFigures = _figuresListsContainerService.GetListOfFigures(FigureListContainerId.Scroll);

            var figuresScrollView =
                _currentLevelDataProvider.CurrentLevelData.GetPrefabsComponent<FiguresScrollView>(Prefab.FiguresScroll);
            
           var figuresScrollPresenter = new FiguresScrollPresenter(
               listOfFigures,
               _figureUIFactory,
               figuresScrollView,
               _figureDataFactory
               );
           
           _dragFigureDataCreators.Add(figuresScrollPresenter);
           _objectsToActivate.Add(figuresScrollView.gameObject);
        }

        private void InitializeTowerPresenter()
        {
            var towerView = _towerViewFactory.Create();
            var listOfFigures = _figuresListsContainerService.GetListOfFigures(FigureListContainerId.Tower);
            
            var towerPresenter = new TowerPresenter(listOfFigures, towerView, _figureSpriteViewFactory);
            
            _dragFigureDataCreators.Add(towerPresenter);
            _objectsToActivate.Add(towerView.gameObject);
        }

        private void InitializeBlackHole()
        {
            var blackHoleView = _blackHoleFactory.Create();
            var blackHolePresenter = new BlackHolePresenter(blackHoleView, _figureSpriteViewFactory);
        }

        public void Exit()
        {
            if(_wasInitialized == false) return;
            
            _compositeDisposable?.Dispose();
            _objectsToActivate.ForEach(gameObjectToActivate => 
                gameObjectToActivate.gameObject.SetActive(false));
        }

        public void SetStateMachine(GameStateMachine stateMachine)
        {
        }
    }
}