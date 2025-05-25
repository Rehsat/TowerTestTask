using System;
using System.Collections.Generic;
using System.Linq;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.UI;
using Game.Services.DragAndDrop;
using Game.Services.Input;
using Zenject;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Core.Figures.View.UI
{
    public class FiguresScrollPresenter : IDisposable, IDragFigureDataCreator
    {
        private readonly IListOfFiguresData _listOfScrollFigures;
        private readonly IFactory<FigureData, FigureUI> _figureUIFactory;
        private readonly ICollectionView<FigureUI> _figureUICollectionView;
        private readonly IInputService _inputService;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;

        private Dictionary<FigureData, FigureUI> _figureUis;
        private CompositeDisposable _compositeDisposable;
        private CompositeDisposable _currentInteractedFigureDisposable;
        private ReactiveEvent<FigureData> _onPlayerInteractFigureView;
        private ReactiveTrigger _onFigureInteractCompleted;
        private ReactiveEvent<DragFigureData> _onNewDragFigureData;
        
        public IReadOnlyReactiveEvent<DragFigureData> OnNewDragFigureData => _onNewDragFigureData;
        public FiguresScrollPresenter(
            IListOfFiguresData listOfScrollFigures,
            IFactory<FigureData, FigureUI> figureUIFactory,
            ICollectionView<FigureUI> figureUICollectionView,
            IInputService inputService,
            IFactory<FigureConfig, FigureData> figureDataFactory) // Для этой фабрики надо бы выделить отдельный класс,
                                                                  // // который будет оборачивать этот. Но для тестового думаю сойдет
        {
            _listOfScrollFigures = listOfScrollFigures;
            _figureUIFactory = figureUIFactory;
            _figureUICollectionView = figureUICollectionView;
            _inputService = inputService;
            _figureDataFactory = figureDataFactory;
            
            _onNewDragFigureData = new ReactiveEvent<DragFigureData>();
            _figureUis = new Dictionary<FigureData, FigureUI>();
            _onPlayerInteractFigureView = new ReactiveEvent<FigureData>();
            _onFigureInteractCompleted =new ReactiveTrigger();
            _compositeDisposable = new CompositeDisposable();
            
            Initialize();
        }

        private void Initialize()
        {
            _onPlayerInteractFigureView
                .SubscribeWithSkip(HandleInteractWithFigure)
                .AddTo(_compositeDisposable);
            
            foreach (var figureData in _listOfScrollFigures.FigureDatas)
                SpawnView(figureData);
            
            _listOfScrollFigures.FigureDatas
                .ObserveAdd()
                .Subscribe(newData => SpawnView(newData.Value))
                .AddTo(_compositeDisposable);
            
            _figureUICollectionView
                .SetListOfObjects(_figureUis.Values.ToList());

            _onFigureInteractCompleted
                .SubscribeWithSkip((() => 
                _currentInteractedFigureDisposable?.Dispose()))
                .AddTo(_compositeDisposable);
        }

        private void HandleInteractWithFigure(FigureData figureData)
        {
            if (figureData == null) throw new Exception("called null figure data");
            
            _currentInteractedFigureDisposable?.Dispose();
            _currentInteractedFigureDisposable = new CompositeDisposable();
            
            _inputService.OnInputUpdate
                .Subscribe((() => TryStartDragFigure(figureData)))
                .AddTo(_currentInteractedFigureDisposable);
        }

        private void TryStartDragFigure(FigureData figureData)
        {
            var yDeltaChangeTolerance = 1;
            var deltaDifference = _inputService.PointerDelta.y - Math.Abs(_inputService.PointerDelta.x);
            var userTriedToDrag = _inputService.PointerDelta.y > yDeltaChangeTolerance;
            Debug.LogError(_inputService.PointerDelta.y);
            if (userTriedToDrag)
                StartDragFigure(figureData);
        }

        private IInteractEnabable TryDisableCurrentRootInteraction()
        {
            IInteractEnabable interactableRootUI = null;
            if (_figureUICollectionView is IInteractEnabable interactable)
            {
                interactableRootUI = interactable;
                interactableRootUI.SetInteractEnableState(false);
            }

            return interactableRootUI;
        }

        private void StartDragFigure(FigureData figureData)
        {
            _currentInteractedFigureDisposable?.Dispose();
            var disabledInteractableRoot = 
                TryDisableCurrentRootInteraction();
            
            void OnComplete(DropResult dropResult) => 
                disabledInteractableRoot?.SetInteractEnableState(true);
            
            var newData = _figureDataFactory.Create(figureData.Config);
            var dragFigureData = new DragFigureData(newData, OnComplete, DragFigureSource.Scroll);
            _onNewDragFigureData.Notify(dragFigureData);
        }

        private void SpawnView(FigureData figureData)
        {
            if (_figureUis.ContainsKey(figureData))
            {
                Debug.LogError("Figure data duplicates");
                return;
            }
            
            var figureView = _figureUIFactory.Create(figureData);
            figureView.SetInteractableData(figureData, 
                _onPlayerInteractFigureView, 
                _onFigureInteractCompleted);
            _figureUis.Add(figureData, figureView);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onPlayerInteractFigureView?.Dispose();
        }
    }
}