using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Services.DragAndDrop;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Core.Figures.Tower
{
    public class TowerPresenter : IDragFigureDataCreator, IDisposable
    {
        private readonly IListOfFiguresData _listOfFiguresData;
        private readonly ITowerView _towerView;
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;

        private FiguresAnimator _figuresAnimator;
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<DragFigureData> _onNewDragFigureData;
        private ReactiveEvent<FigureData> _onStartDragFigure;

        private List<FigureSpriteView> _figureSpriteViews;
        private Dictionary<FigureData, FigureSpriteView> _figureSpriteViewsByData;

        public IReadOnlyReactiveEvent<DragFigureData> OnNewDragFigureData => _onNewDragFigureData;
        
        public TowerPresenter(
            IListOfFiguresData listOfFiguresData, 
            ITowerView towerView,
            IFactory<FigureData, FigureSpriteView> figureSpriteViewFactory)
        {
            _listOfFiguresData = listOfFiguresData;
            _towerView = towerView;
            _figureSpriteViewFactory = figureSpriteViewFactory;

            _figuresAnimator = new FiguresAnimator();
            _compositeDisposable = new CompositeDisposable();
            _figureSpriteViews = new List<FigureSpriteView>();
            _figureSpriteViewsByData = new Dictionary<FigureData, FigureSpriteView>();
            _onStartDragFigure = new ReactiveEvent<FigureData>();
            _onNewDragFigureData = new ReactiveEvent<DragFigureData>();
            
            Initialize();
        }

        private void Initialize()
        {
            _listOfFiguresData.FigureDatas.ToList().ForEach(AddDataToViews);
            
            _listOfFiguresData.FigureDatas
                .ObserveAdd()
                .Subscribe(figureData => AddDataToViews(figureData.Value))
                .AddTo(_compositeDisposable);
            
            _listOfFiguresData.FigureDatas
                .ObserveRemove()
                .Subscribe(figureData =>
                    RemoveData(figureData.Index))
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
            draggable.OnDragComplete(DropResult.Fail);
        }

        private void HandleNewDragFigureData(DraggableView draggableView, DragFigureData dragFigureData)
        {
            //Тут можно проверить пункт из ТЗ про обновляемость (про то что можно поменять логику на "ставить кубики только того же цвета")
            // проверяем есть ли что-то в списке фигур и если есть, то сравниваем цвет даты ее и дропнутой. 
            if (_figureSpriteViewsByData.ContainsKey(dragFigureData.FigureData))
            {
                draggableView.OnDragComplete(DropResult.Fail);
                return;
            }
            
            _listOfFiguresData.AddData(dragFigureData.FigureData);
            draggableView.OnDragComplete(DropResult.Success);
        }

        private void AddDataToViews(FigureData figureData)
        {
            if (_figureSpriteViewsByData.ContainsKey(figureData))
            {
                var buggedView = _figureSpriteViewsByData[figureData];
                buggedView.name = GetHashCode().ToString();
                throw new Exception($"You tried to add already added data {buggedView.name}");
            }

            var view = _figureSpriteViewFactory.Create(figureData);
            view.SetInteractableData(figureData, _onStartDragFigure);
            _figureSpriteViewsByData.Add(figureData, view);
            AddFigureViewToList(figureData, view);
        }

        private void AddFigureViewToList(FigureData figureData, FigureSpriteView figureSpriteView)
        {
            if (_figureSpriteViews.Count > 0)
                ConnectViewToLast(figureData, figureSpriteView);
            else
                _towerView.PlaceFirstViewTransform(figureSpriteView.transform);
            
            
            _towerView.SetLastViewTransform(figureSpriteView.transform);
            _figureSpriteViews.Add(figureSpriteView);
            figureSpriteView.DoPlaceAnimation();
            _figuresAnimator.DoJumpAnimation(figureSpriteView.transform);
        }

        private void ConnectViewToLast(FigureData figureData, FigureSpriteView figureSpriteView)
        {
            var currentLastView = _figureSpriteViews[^1];
            var figureToConnectWith = currentLastView.transform;
            var figureToConnect = figureSpriteView.transform;
            var xPosition = figureToConnectWith.localScale.x * (figureData.XMovementPercent / 100);
            
            figureToConnect.parent = figureToConnectWith;
            figureToConnect.localPosition = 
                new Vector2(xPosition, figureToConnectWith.localScale.y) * 2;
        }

        private void RemoveData(int index)
        {
            if (_figureSpriteViews.Count <= index)
            {
                Debug.LogError($"List of views in tower was corrupted");
                return;
            }

            var viewToRemove = _figureSpriteViews[index];
            var viewToRemoveCurrentParent = viewToRemove.transform.parent;
            var isLastFigure = _figureSpriteViews.Count == index - 1;
            
            if (isLastFigure == false) // можно !isLastFigure, но так заметней
            {
                var nextViewInTower = _figureSpriteViews[index];
                var nextViewTransform = nextViewInTower.transform;
                var nextViewPosition = nextViewTransform.position;
                var newNextViewPosition = new Vector2(nextViewPosition.x, nextViewPosition.y - viewToRemove.transform.localScale.y);
                nextViewTransform.parent = viewToRemoveCurrentParent;
                _figuresAnimator.DoDropAnimation(nextViewTransform, newNextViewPosition);
            }

            viewToRemove.Dispose();
            Object.Destroy(viewToRemove.gameObject); //Можно будет добавить обжект пул в обновлениях, на данный момент он кажется мне оверинжениренгом. Слишком мало объектов уничтожается
        }

        private void OnInteractWithFigure(FigureData figureData)
        {
            var dragData = new DragFigureData(figureData, OnComplete, DragFigureSource.Tower);
            _onNewDragFigureData.Notify(dragData);

            void OnComplete(DropResult result)
            {
                if (result == DropResult.Success)
                    _listOfFiguresData.RemoveData(figureData);
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onNewDragFigureData?.Dispose();
        }
    }
}