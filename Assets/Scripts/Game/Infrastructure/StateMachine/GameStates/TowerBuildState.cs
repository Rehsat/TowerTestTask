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
        private readonly IFactory<FiguresScrollView> _figureScrollViewFactory;
        private readonly IFactory<FiguresScrollPresenter> _figuresScrollPresenterFactory;
        private readonly IFactory<TowerPresenter> _towerPresenterFactory;
        private readonly IFactory<BlackHolePresenter> _blackHolePresenterFactory;

        private readonly IDragDataHandleService _dragDataHandleService;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
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
            ICurrentLevelDataProvider currentLevelDataProvider,
            ILogService logService,
            
            IFactory<FiguresScrollPresenter> figuresScrollPresenterFactory,
            IFactory<TowerPresenter> towerPresenterFactory,
            IFactory<BlackHolePresenter> blackHolePresenterFactory
            )
        {
            _logService = logService;
            _dragDataHandleService = dragDataHandleService;
            _currentLevelDataProvider = currentLevelDataProvider;
            _figuresScrollPresenterFactory = figuresScrollPresenterFactory;
            _towerPresenterFactory = towerPresenterFactory;
            _blackHolePresenterFactory = blackHolePresenterFactory;

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
            var towerPresenter = _towerPresenterFactory.Create();
            var towerLogger = new TowerBuildLogger(towerPresenter);
            
            _dragFigureDataCreators.Add(towerPresenter);
            _logsCreators.Add(towerLogger);
        }

        private void InitializeBlackHole()
        {
            var blackHolePresenter =_blackHolePresenterFactory.Create();
            
            _logsCreators.Add(blackHolePresenter);
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