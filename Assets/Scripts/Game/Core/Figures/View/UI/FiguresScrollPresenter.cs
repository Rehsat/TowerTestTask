using System;
using System.Collections.Generic;
using System.Linq;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.UI;
using Game.Services.DragAndDrop;
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

        private Dictionary<FigureData, FigureUI> _figureUis;
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<FigureData> _onPlayerInteractFigureView;
        private ReactiveEvent<DragFigureData> _onNewDragFigureData;
        
        public IReadOnlyReactiveEvent<DragFigureData> OnNewFigureData => _onNewDragFigureData;
        public FiguresScrollPresenter(
            IListOfFiguresData listOfScrollFigures,
            IFactory<FigureData, FigureUI> figureUIFactory,
            ICollectionView<FigureUI> figureUICollectionView)
        {
            _listOfScrollFigures = listOfScrollFigures;
            _figureUIFactory = figureUIFactory;
            _figureUICollectionView = figureUICollectionView;
            _onNewDragFigureData = new ReactiveEvent<DragFigureData>();
            
            _figureUis = new Dictionary<FigureData, FigureUI>();
            _onPlayerInteractFigureView = new ReactiveEvent<FigureData>();
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
            
            _figureUICollectionView.SetListOfObjects(_figureUis.Values.ToList());
        }

        private void HandleInteractWithFigure(FigureData figureData)
        {
            if (figureData == null)
            {
                Debug.LogError("Called null data");
                return;
            }
            
            IInteractEnabable interactableRootUI = null;
            if (_figureUICollectionView is IInteractEnabable interactable)
            {
                interactableRootUI = interactable;
                interactableRootUI.SetInteractEnableState(false);
            }
            void OnComplete(DropResult dropResult)
            {
                interactableRootUI?.SetInteractEnableState(true);
            }

            StartDragFigure(figureData, OnComplete);
        }

        private void StartDragFigure(FigureData figureData, Action<DropResult> onComplete)
        {
            var dragFigureData = new DragFigureData(figureData, onComplete);
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
            figureView.SetInteractableData(figureData, _onPlayerInteractFigureView);
            _figureUis.Add(figureData, figureView);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onPlayerInteractFigureView?.Dispose();
        }
    }
}