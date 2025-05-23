﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Services.DragAndDrop;
using Game.Services.LogService;
using Game.Services.OutOfScreenCheck;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Core.Figures.Tower
{
    public class TowerPresenter : IDragFigureDataCreator, IDisposable, ILogsCreator
    {
        private readonly IListOfFiguresData _listOfFiguresData;
        private readonly ITowerView _towerView;
        private readonly IOutOfScreenCheckService _ofScreenCheckService;
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;

        private FiguresAnimator _figuresAnimator;
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<DragFigureData> _onNewDragFigureData;
        private ReactiveEvent<LocalizableLogData> _onNewLogs;
        private ReactiveEvent<FigureData> _onStartDragFigure;

        private List<FigureSpriteView> _figureSpriteViews;

        public IReadOnlyReactiveEvent<DragFigureData> OnNewDragFigureData => _onNewDragFigureData;
        public IReadOnlyReactiveEvent<LocalizableLogData> OnNewLogs => _onNewLogs;
        
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

            _figuresAnimator = new FiguresAnimator();
            _compositeDisposable = new CompositeDisposable();
            _figureSpriteViews = new List<FigureSpriteView>();
            _onStartDragFigure = new ReactiveEvent<FigureData>();
            _onNewDragFigureData = new ReactiveEvent<DragFigureData>();
            _onNewLogs = new ReactiveEvent<LocalizableLogData>();

            Initialize();
        }

        private void Initialize()
        {
            _listOfFiguresData.FigureDatas.ToList().ForEach(ShowDataInTower);
            
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
            draggable.OnDragComplete(DropResult.Fail);
        }

        private void HandleNewDragFigureData(DraggableView draggableView, DragFigureData dragFigureData)
        {
            
            //Тут можно проверить пункт из ТЗ про обновляемость (про то что можно поменять логику на "ставить кубики только того же цвета")
            // проверяем есть ли что-то в списке фигур и если есть, то сравниваем цвет даты ее и дропнутой. 
            if (_ofScreenCheckService.IsObjectOutOfScreen(_towerView.DropContainerTransform))
            {
                draggableView.OnDragComplete(DropResult.Fail);
                LogFigureOutOfRange();
                return;
            }
            
            _listOfFiguresData.AddData(dragFigureData.FigureData);
            draggableView.OnDragComplete(DropResult.Success);
        }

        private void ShowDataInTower(FigureData figureData)
        {
            _figuresAnimator.KillCurrentAnimation();
            
            var view = _figureSpriteViewFactory.Create(figureData);
            view.name = view.name + _figureSpriteViews.Count;
            view.SetInteractableData(figureData, _onStartDragFigure);
            
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
            _figuresAnimator.DoJumpAnimation(figureSpriteView.transform);
            LogNewFigureAdded();
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

        private void OnDataRemoved(int index)
        {
            if (IsIndexValid(index) == false) 
                return;

            var viewToRemove = _figureSpriteViews[index];
            var isLastFigure = _figureSpriteViews.Count - 1 == index ;
            _figuresAnimator.KillCurrentAnimation();

            if (isLastFigure)
                HandleLastFigureRemoval(index);
            else
                HandleNonLastFigureRemoval(index, viewToRemove);
            
            _figureSpriteViews.RemoveAt(index);
            viewToRemove.ReturnToPool();
            LogFigureRemove(index + 1);
        }
        private bool IsIndexValid(int index)
        {
            if (_figureSpriteViews.Count > index) 
                return true;
    
            Debug.LogError("List of views in tower was corrupted");
            return false;
        }
        private void HandleLastFigureRemoval(int index)
        {
            Transform newLastFigure = index > 0 
                ? _figureSpriteViews[index - 1].transform 
                : null;
    
            _towerView.SetLastViewTransform(newLastFigure);
        }
        private void HandleNonLastFigureRemoval(int index, FigureSpriteView viewToRemove)
        {
            var nextViewInTower = _figureSpriteViews[index + 1];
            var nextViewTransform = nextViewInTower.transform;
            nextViewTransform.parent = viewToRemove.transform.parent;
                
            var nextViewPosition = nextViewTransform.localPosition;
            var newNextViewPosition = new Vector2(nextViewPosition.x, nextViewPosition.y - viewToRemove.transform.localScale.y * 2);
            _figuresAnimator.DoDropAnimation(nextViewTransform, newNextViewPosition);
        }

        private void OnInteractWithFigure(FigureData figureData)
        {
            var dragData = new DragFigureData(figureData, OnComplete, DragFigureSource.Tower);
            _onNewDragFigureData.Notify(dragData);
            _listOfFiguresData.RemoveData(figureData);
            void OnComplete(DropResult result)
            {
            }
        }

        private void LogFigureRemove(int index)
        {
            var listOfLogStrings = new List<string>()
            {
                "Фигура под номером",
                index.ToString(),
                "была убрана"
            };
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }

        private void LogFigureOutOfRange()
        {
            var listOfLogStrings = new List<string>(){ "Фигура не помещается в границах экрана" };
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }

        private void LogNewFigureAdded()
        {
            var listOfLogStrings = new List<string>(){"Была поставлена новая фиугра"};
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onNewDragFigureData?.Dispose();
        }

    }
}