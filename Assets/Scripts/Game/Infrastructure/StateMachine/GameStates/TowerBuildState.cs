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
using Game.Core.UI.Logs;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.CurrentLevelData;
using Game.Services.Canvases;
using Game.Services.DragAndDrop;
using Game.Services.FiguresCollections;
using Game.Services.LogService;
using Game.Services.LogService.Loggers;
using Game.Services.OutOfScreenCheck;
using Infrastructure.StateMachine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.StateMachine.GameStates
{
    public class TowerBuildState : IGameState
    {
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IFactory<TowerView> _towerViewFactory;
        private readonly IFactory<BlackHoleView> _blackHoleFactory;
        private readonly IFactory<FiguresScrollPresenter> _figuresScrollPresenterFactory;

        private readonly IDragDataHandleService _dragDataHandleService;
        private readonly IFiguresListsProvider _figuresListsProvider;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly IOutOfScreenCheckService _ofScreenCheckService;
        private readonly ILogService _logService;

        private List<ILogsCreator> _logsCreators;
        private List<IDragFigureDataCreator> _dragFigureDataCreators;
        private List<GameObject> _objectsToActivate;
        
        private CompositeDisposable _compositeDisposable;

        private bool _wasInitialized;
        //С одной стороный тут биндятся и инициаализируются все нужные для стейта вещи и вроде все, но как будто нарушается SRP (очень много зависимостей)
        // точно не знаю как это тут поправить, хотелсь бы совета более опытного человека
        public TowerBuildState(
            IDragDataHandleService dragDataHandleService,
            IFiguresListsProvider figuresListsProvider,
            ICurrentLevelDataProvider currentLevelDataProvider,
            IOutOfScreenCheckService ofScreenCheckService,
            ILogService logService,
            
            IFactory<FigureData, FigureSpriteView> figureSpriteViewFactory,
            IFactory<TowerView> towerViewFactory,
            IFactory<BlackHoleView> blackHoleFactory,
            IFactory<FiguresScrollPresenter> figuresScrollPresenterFactory
            )
        {
            _logService = logService;
            _dragDataHandleService = dragDataHandleService;
            _figuresListsProvider = figuresListsProvider;
            _currentLevelDataProvider = currentLevelDataProvider;
            _ofScreenCheckService = ofScreenCheckService;
            _figureSpriteViewFactory = figureSpriteViewFactory;
            _towerViewFactory = towerViewFactory;
            _blackHoleFactory = blackHoleFactory;
            _figuresScrollPresenterFactory = figuresScrollPresenterFactory;
            
            _dragFigureDataCreators = new List<IDragFigureDataCreator>();
            _objectsToActivate = new List<GameObject>();
            _logsCreators = new List<ILogsCreator>();
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
            
            _logsCreators.ForEach(creator =>
            {
                creator.OnNewLogs
                    .SubscribeWithSkip(_logService.OnNewLogData)
                    .AddTo(_compositeDisposable);
            });
        }

        private void Initialize()
        {
            InitializeScrollPresenter();
            InitializeTowerPresenter();
            InitializeBlackHole();
            InitializeLogger();
            _wasInitialized = true;
        }
        private void InitializeScrollPresenter()
        {
            var figuresScrollPresenter = _figuresScrollPresenterFactory.Create();
            _dragFigureDataCreators.Add(figuresScrollPresenter);
        }

        private void InitializeTowerPresenter()
        {
            var towerView = _towerViewFactory.Create();
            var listOfFigures = _figuresListsProvider.GetListOfFigures(FigureListContainerId.Tower);
            
            var towerPresenter = new TowerPresenter(listOfFigures, 
                towerView,
                _ofScreenCheckService,
                _figureSpriteViewFactory);
            
            var towerLogger = new TowerBuildLogger(towerPresenter);
            
            _dragFigureDataCreators.Add(towerPresenter);
            _logsCreators.Add(towerLogger);
            _objectsToActivate.Add(towerView.gameObject);
        }

        private void InitializeBlackHole()
        {
            var blackHoleView = _blackHoleFactory.Create();
            var blackHolePresenter = new BlackHolePresenter(blackHoleView, _figureSpriteViewFactory);
            
            _logsCreators.Add(blackHolePresenter);
            _objectsToActivate.Add(blackHoleView.gameObject);
        }

        private void InitializeLogger()
        {
            var loggerView = 
                _currentLevelDataProvider.CurrentLevelData.GetPrefabsComponent<LogView>(Prefab.LogView);
            new LogsPresenter(_logService, loggerView);
            _objectsToActivate.Add(loggerView.gameObject);
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