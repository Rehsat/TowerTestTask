using System;
using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure;
using Game.Services.Cameras;
using Game.Services.Canvases;
using Game.Services.Input;
using Game.Services.RaycastService;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Services.DragAndDrop
{
    public class DragAndDropService : IDragService, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;

        private readonly ReactiveProperty<IDraggable> _currentDraggable;
        private readonly CompositeDisposable _compositeDisposable;
        private readonly RectTransform _canvasRectTransform;
        private CompositeDisposable _draggableCompositeDisposable;
        private bool IsDragging => _currentDraggable.Value != null;

        public DragAndDropService(
            IInputService inputService, 
            ICanvasLayersProvider canvasLayersProvider,
            IRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            _currentDraggable = new ReactiveProperty<IDraggable>();
            _compositeDisposable = new CompositeDisposable();
            _canvasRectTransform = canvasLayersProvider
                .GetCanvasByLayer(CanvasLayer.DragAndDrop)
                .GetComponent<RectTransform>();
            
            InitializeSubscribes();
        }

        private void InitializeSubscribes()
        {
            _currentDraggable
                .Subscribe(HandleNewDraggable)
                .AddTo(_compositeDisposable);
            
            _inputService.OnDragActionStateChanged
                .SubscribeWithSkip(state =>
                {
                    if (state == ActionState.Completed) 
                        OnDragEnded();
                })
                .AddTo(_compositeDisposable);
        }

        public void StartDrag(IDraggable draggable)
        {
            if (_currentDraggable.Value != null)
                OnDragEnded();
            
            _currentDraggable.Value = draggable;
        }

        private void HandleNewDraggable(IDraggable newDraggable)
        {
            if (newDraggable == null)
            {
                _draggableCompositeDisposable?.Dispose();
                return;
            }
            _draggableCompositeDisposable = new CompositeDisposable();

            newDraggable.TransformToDrag.SetParent(_canvasRectTransform.transform, false);
            _inputService.OnInputUpdate
                .Subscribe(OnInputUpdate)
                .AddTo(_draggableCompositeDisposable);
            
            newDraggable.OnDragStart();
        }
        
        private void OnInputUpdate()
        {
            if (IsDragging)
                UpdateDraggablePosition(_currentDraggable.Value.TransformToDrag);
        }

        private void UpdateDraggablePosition(RectTransform target)
        {
            var screenPosition = _inputService.PointerPosition;
            
            var canvasSize = _canvasRectTransform.sizeDelta;
            var normalizedPosition = new Vector2(
                screenPosition.x / Screen.width * canvasSize.x - canvasSize.x/2,
                screenPosition.y / Screen.height * canvasSize.y - canvasSize.y/2);

            target.anchoredPosition = normalizedPosition;
        }
        
        private void OnDragEnded()
        {
            FindAndNotifyDropContainer();
        }

        private void FindAndNotifyDropContainer()
        {
            var dropScreenPosition = _inputService.PointerPosition;
            
            if(_raycastService.TryGetComponentInRaycastHit<IDropContainer>
                (dropScreenPosition, out var container))
                TryDropCurrentDraggable(container);
            else
                TryDropCurrentDraggable(null);
        }

        private void TryDropCurrentDraggable(IDropContainer dropContainer)
        {
            if(_currentDraggable.Value == null) 
                return;

            if (dropContainer == null)
                _currentDraggable.Value.DragComplete(DropResult.Fail);
            else
                HandleDropInContainer(dropContainer, _currentDraggable.Value);
            
            _draggableCompositeDisposable?.Dispose();
            _currentDraggable.Value = null;
        }

        private void HandleDropInContainer(IDropContainer container, IDraggable draggable)
        {
            draggable.DoSuccessDropAnimation((() =>
                container.OnDrop(draggable)));
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _draggableCompositeDisposable?.Dispose();
            TryDropCurrentDraggable(null);
        }
    }
}