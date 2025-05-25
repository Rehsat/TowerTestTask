using System;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Services.DragAndDrop;
using Game.Services.LogService;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Core.BlackHole
{
    public class BlackHolePresenter : IDisposable
    {
        private readonly IBlackHoleView _blackHoleView;
        private readonly IFactory<FigureData, FigureSpriteView> _figuresFactory;
        private readonly ReactiveEvent<DragFigureData> _onFigureSucked;
        private readonly CompositeDisposable _compositeDisposable;
        public ReactiveEvent<DragFigureData> OnFigureSucked => _onFigureSucked;

        public BlackHolePresenter(
            IBlackHoleView blackHoleView, 
            IFactory<FigureData, FigureSpriteView> figuresFactory)
        {
            _blackHoleView = blackHoleView;
            _figuresFactory = figuresFactory;
            
            _onFigureSucked = new ReactiveEvent<DragFigureData>();
            _compositeDisposable = new CompositeDisposable();
            _blackHoleView.OnDroppedNewObject
                .SubscribeWithSkip(OnDroppedNewObject)
                .AddTo(_compositeDisposable);
        }

        private void OnDroppedNewObject(IDraggable draggable)
        {
            if(draggable is DraggableView draggableView)
                if (draggableView.DragData is DragFigureData dragFigureData)
                {
                    OnNewDragFigureData(draggableView, dragFigureData);
                    return;
                }
            
            draggable.DragComplete(DropResult.Fail);
        }

        private void OnNewDragFigureData(DraggableView draggableView, DragFigureData dragFigureData)
        { 
            if (dragFigureData.Source == DragFigureSource.Scroll)
            {
                draggableView.DragComplete(DropResult.Fail);
                return;
            }
            
            var newFigureSprite = _figuresFactory.Create(dragFigureData.FigureData);
            newFigureSprite.SetMaskMode(SpriteMaskInteraction.VisibleInsideMask); // думаю в теории это можно перенести во вьюху
            _blackHoleView.DoSuckAnimation(newFigureSprite.transform, OnSuckComplete);
            draggableView.DragComplete(DropResult.Success);
            
            void OnSuckComplete()
            {
                newFigureSprite.ReturnToPool();
                _onFigureSucked.Notify(dragFigureData);
            }
        }
        
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

    }
}