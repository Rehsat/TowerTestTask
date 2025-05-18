using System;
using System.Collections.Generic;
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
    public class FiguresScrollPresenter : IDisposable
    {
        private readonly IListOfFiguresData _listOfScrollFigures;
        private readonly IFactory<FigureData, FigureUI> _figureUIFactory;
        private readonly IScrollView<FigureUI> _figureUIScrollView;
        private readonly IDragService _dragService;

        private Dictionary<FigureData, FigureUI> _figureUis;
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<FigureData> _onPlayerInteractFigureView;

        public FiguresScrollPresenter(
            IListOfFiguresData listOfScrollFigures,
            IFactory<FigureData, FigureUI> figureUIFactory,
            IScrollView<FigureUI> figureUIScrollView,
            IDragService dragService)
        {
            _listOfScrollFigures = listOfScrollFigures;
            _figureUIFactory = figureUIFactory;
            _figureUIScrollView = figureUIScrollView;
            _dragService = dragService;
            
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
            
            _listOfScrollFigures.FigureDatas
                .ObserveAdd()
                .Subscribe(newData => SpawnView(newData.Value))
                .AddTo(_compositeDisposable);
        }

        private void HandleInteractWithFigure(FigureData figureData)
        {
            
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

    public class DragFigureData : IDraggable
    {
        public RectTransform TransformToDrag { get; }

        public DragFigureData(Action onFail, Action onComplete)
        {
            
        }
        public void OnDragStart()
        {
            
        }

        public void OnDragComplete()
        {
        }
    }
}