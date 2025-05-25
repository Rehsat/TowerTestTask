using System;
using System.Collections.Generic;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;
using Game.Services.Cameras;
using Game.Services.LogService;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Services.DragAndDrop
{
    public class DragDataHandleService : IDragDataHandleService, ILogsCreator
    {
        private readonly IFactory<FigureData, FigureUI> _figureUiFactory;
        private readonly ICameraService _cameraService;
        private readonly IDragService _dragService;
        private readonly ParticleSystem _explosionParticle;
        
        private ReactiveEvent<LocalizableLogData> _onNewLogs;
        public IReadOnlyReactiveEvent<LocalizableLogData> OnNewLogs => _onNewLogs;
        public DragDataHandleService(
            IFactory<FigureData, FigureUI> figureUiFactory,
            IPrefabsProvider prefabProvider,
            ICameraService cameraService,
            IDragService dragService)
        {
            _figureUiFactory = figureUiFactory;
            _cameraService = cameraService;
            _dragService = dragService;
            _explosionParticle = prefabProvider.GetPrefabsComponent<ParticleSystem>(Prefab.ExplosionParticle);
            _onNewLogs = new ReactiveEvent<LocalizableLogData>();
        }
        public void HandleDragData(IDragData dragData)
        {
            //тут может быть словарь с разными классами/методами для хендла различных дат, но у меня она только одна, поэтому так
            if (dragData is DragFigureData dragFigureData)
                HandleFigureDragData(dragFigureData);
            else
                dragData.SendCallback(DropResult.Fail);
        }

        private void HandleFigureDragData(DragFigureData dragFigureData)
        {
            var figureUI = _figureUiFactory.Create(dragFigureData.FigureData);
            var figureRect = figureUI.GetComponent<RectTransform>();
            
            var dragView = new DraggableView(dragFigureData, figureRect, OnDropComplete, GetViewSuccessDropAnimation);
            _dragService.StartDrag(dragView);
        }

        private void OnDropComplete(DraggableView draggableView, DropResult result)
        {
            if (result == DropResult.Success) return;

            var particleSpawnPosition = _cameraService.MainCamera.ScreenToWorldPoint(draggableView.DragEndPosition);
            var particle = Object.Instantiate(_explosionParticle, particleSpawnPosition, Quaternion.identity);
            Object.Destroy(particle, particle.main.duration + 1);
            LogFigureDragFail();
        }

        private Sequence GetViewSuccessDropAnimation(DraggableView draggableView, DropResult dropResult)
        {
            if (draggableView.DragData is DragFigureData dragFigureData &&
                dragFigureData.Source != DragFigureSource.Scroll) return null;
            
            var sequence = DOTween.Sequence();
            var droppableTransform = draggableView.TransformToDrag;

            droppableTransform.gameObject.SetActive(true);
            droppableTransform.position = draggableView.DragStartPosition;

            // немного магических чисел, можно будет добавить конфиг
            var setStartPositionTween = droppableTransform
                .DOMove(draggableView.DragStartPosition, 0);
            
            var startHideTween = 
                droppableTransform.DOScale(0, 0);

            var appearAnimation =
                droppableTransform
                    .DOScale(1, 0.35f)
                    .SetEase(Ease.OutBack);
                
            var moveToDropPosition = droppableTransform
                .DOMove(draggableView.DragEndPosition, 0.5f)
                .SetEase(Ease.OutCubic);

            var hideTween = draggableView.TransformToDrag
                .DOScale(0, 0.2f)
                .SetEase(Ease.InCubic);

            sequence
                .Append(startHideTween)
                .Append(setStartPositionTween)
                .Append(moveToDropPosition)
                .Join(appearAnimation)
                .Append(hideTween);

            return sequence;
        }

        private void StartDrag(DraggableView dragView)
        {
            
        }
        private void LogFigureDragFail()
        {
            var listOfLogStrings = new List<string>(){"Ты перетащил фигуру в неподходящее место"};
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }
    }
}