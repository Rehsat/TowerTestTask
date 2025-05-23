﻿using System.Collections.Generic;
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
            //тут может быть словарь с разными классами/методами для хендла различных дат, но у меня она только одна, поэтому пока так
            if (dragData is DragFigureData dragFigureData)
                HandleFigureDragData(dragFigureData);
            else
                dragData.SendCallback(DropResult.Fail);
        }

        private void HandleFigureDragData(DragFigureData dragFigureData)
        {
            var figureUI = _figureUiFactory.Create(dragFigureData.FigureData);
            var figureRect = figureUI.GetComponent<RectTransform>();
            if (figureRect == null)
            {
                dragFigureData.SendCallback(DropResult.Fail);
                return;
            }
                
            var dragView = new DraggableView(dragFigureData, figureRect, OnComplete);
            _dragService.StartDrag(dragView);

            void OnComplete(Vector3 completePosition, DropResult result)
            {
                if(result == DropResult.Success) return;
                
                var particleSpawnPosition = _cameraService.MainCamera.ScreenToWorldPoint(completePosition);
                var particle = Object.Instantiate(_explosionParticle, particleSpawnPosition, Quaternion.identity);
                Object.Destroy(particle, particle.main.duration+1);
                LogFigureDragFail();
            }
        }
        private void LogFigureDragFail()
        {
            var listOfLogStrings = new List<string>(){"Ты перетащил фигуру в неподходящее место"};
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }
    }
}