using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Services.DragAndDrop;
using Game.Services.OutOfScreenCheck;
using UnityEngine;
using Zenject;

namespace Game.Core.Figures.Tower
{
    public class TowerPresenter : IDragFigureDataCreator, IDisposable
    {
        private readonly IListOfFiguresData _listOfFiguresData;
        private readonly ITowerView _towerView;
        private readonly FiguresTowerBuilder _towerBuilder;
        private readonly IOutOfScreenCheckService _ofScreenCheckService;
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;

        private FiguresAnimator _figuresAnimator;
        private CompositeDisposable _compositeDisposable;
        private List<FigureSpriteView> _figureSpriteViews;
        
        private ReactiveEvent<DragFigureData> _onNewDragFigureData;
        private ReactiveEvent<FigureData> _onNewFigureAdded;
        private ReactiveEvent<int> _onFigureRemoved;
        private ReactiveEvent<FigureData> _onStartDragFigure;
        private ReactiveTrigger _onFigureOutOfRange;
        public IReadOnlyReactiveTrigger OnFigureOutOfRange => _onFigureOutOfRange;
        public IReadOnlyReactiveEvent<DragFigureData> OnNewDragFigureData => _onNewDragFigureData;
        public IReadOnlyReactiveEvent<FigureData> OnNewFigureAdded => _onNewFigureAdded;
        public IReadOnlyReactiveEvent<int> OnFigureRemoved => _onFigureRemoved;
        
        public TowerPresenter(
            IListOfFiguresData listOfFiguresData, 
            ITowerView towerView,
            IOutOfScreenCheckService ofScreenCheckService,
            IFactory<FigureData, FigureSpriteView> figureSpriteViewFactory)
        {
            _listOfFiguresData = listOfFiguresData;
            _towerView = towerView;
            _ofScreenCheckService = ofScreenCheckService;
            _figureSpriteViewFactory = figureSpriteViewFactory;
            
            _figureSpriteViews = new List<FigureSpriteView>();
            _figuresAnimator = new FiguresAnimator();
            _towerBuilder = new FiguresTowerBuilder(_figuresAnimator, towerView);
            
            _compositeDisposable = new CompositeDisposable();
            _onFigureOutOfRange = new ReactiveTrigger();
            _onStartDragFigure = new ReactiveEvent<FigureData>();
            _onNewFigureAdded = new ReactiveEvent<FigureData>();
            _onNewDragFigureData = new ReactiveEvent<DragFigureData>();
            _onFigureRemoved = new ReactiveEvent<int>();

            Initialize();
        }

        private void Initialize()
        {
            _listOfFiguresData.FigureDatas.ToList().ForEach(ShowDataInTower);
            _figuresAnimator.Enable();
            
            _listOfFiguresData.FigureDatas
                .ObserveAdd()
                .Subscribe(figureData => ShowDataInTower(figureData.Value))
                .AddTo(_compositeDisposable);
            
            _listOfFiguresData.FigureDatas
                .ObserveRemove()
                .Subscribe(figureData =>
                    OnDataRemoved(figureData.Index))
                .AddTo(_compositeDisposable);

            _towerView.OnDroppedNewObject
                .SubscribeWithSkip(HandleNewDroppedObject)
                .AddTo(_compositeDisposable);

            _onStartDragFigure
                .SubscribeWithSkip(OnInteractWithFigure)
                .AddTo(_compositeDisposable);
        }

        private void HandleNewDroppedObject(IDraggable draggable)
        {
            if (draggable is DraggableView draggableView)
                if (draggableView.DragData is DragFigureData dragFigureData)
                {
                    HandleNewDragFigureData(draggableView, dragFigureData);
                    return;
                }
            draggable.DragComplete(DropResult.Fail);
        }

        private void HandleNewDragFigureData(DraggableView draggableView, DragFigureData dragFigureData)
        {
            //Тут можно проверить пункт из ТЗ про обновляемость (про то что можно поменять логику на "ставить кубики только того же цвета")
            // проверяем есть ли что-то в списке фигур и если есть, то сравниваем цвет даты ее и дропнутой. 
            if (_ofScreenCheckService.IsObjectOutOfScreen(_towerView.DropContainerTransform))
            {
                draggableView.DragComplete(DropResult.Fail);
                _onFigureOutOfRange.Notify();
                return;
            }
            
            _listOfFiguresData.AddData(dragFigureData.FigureData);
            draggableView.DragComplete(DropResult.Success);
        }

        private void ShowDataInTower(FigureData figureData)
        {
            var view = _figureSpriteViewFactory.Create(figureData);
            view.SetInteractableData(figureData, _onStartDragFigure);
        
            AddFigureViewToList(figureData, view);
        }

        private void AddFigureViewToList(FigureData figureData, FigureSpriteView figureSpriteView)
        {
            var offset = new Vector2(figureData.XMovementPercent, 0);
            _figureSpriteViews.Add(figureSpriteView);
            _towerBuilder.PlaceNewElementInTower(figureSpriteView.transform, offset);
            _onNewFigureAdded.Notify(figureData);
        }

        private void OnDataRemoved(int index)
        {
            var viewToRemove = _figureSpriteViews[index];
            _towerBuilder.RemoveFromTower(index);
        
            _figureSpriteViews.RemoveAt(index);
            viewToRemove.ReturnToPool();
            _onFigureRemoved.Notify(index);
        }

        private void OnInteractWithFigure(FigureData figureData)
        {
            var dragData = new DragFigureData(figureData, null, DragFigureSource.Tower);
            _onNewDragFigureData.Notify(dragData);
            _listOfFiguresData.RemoveData(figureData);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onNewDragFigureData?.Dispose();
            _onNewFigureAdded?.Dispose();
            _onFigureRemoved?.Dispose();
            _onStartDragFigure?.Dispose();
            _onFigureOutOfRange?.Dispose();
        }
    }
}