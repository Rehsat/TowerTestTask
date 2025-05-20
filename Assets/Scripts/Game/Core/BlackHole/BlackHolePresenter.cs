using System;
using Game.Core.Figures;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Services.DragAndDrop;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Core.BlackHole
{
    public class BlackHolePresenter : IDisposable
    {
        private readonly IBlackHoleView _blackHoleView;
        private readonly IFactory<FigureData, FigureSpriteView> _figuresFactory;
        private CompositeDisposable _compositeDisposable;

        public BlackHolePresenter(
            IBlackHoleView blackHoleView, 
            IFactory<FigureData, FigureSpriteView> figuresFactory)
        {
            _blackHoleView = blackHoleView;
            _figuresFactory = figuresFactory;
            
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
            
            draggable.OnDragComplete(DropResult.Fail);
        }

        private void OnNewDragFigureData(DraggableView draggableView, DragFigureData dragFigureData)
        { 
            if (dragFigureData.Source == DragFigureSource.Scroll)
            {
                draggableView.OnDragComplete(DropResult.Fail);
                return;
            }
            
            var newFigureSprite = _figuresFactory.Create(dragFigureData.FigureData);
            newFigureSprite.SetMaskMode(SpriteMaskInteraction.VisibleInsideMask); // думаю в теории это можно перенести во вьюху
            _blackHoleView.DoSuckAnimation(newFigureSprite.transform, OnSuckComplete);
            
            draggableView.OnDragComplete(DropResult.Success);
            void OnSuckComplete()
            {
                newFigureSprite.ReturnToPool();
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}