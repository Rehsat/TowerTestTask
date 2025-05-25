using System;
using DG.Tweening;
using Game.Services.DragAndDrop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Figures
{
    public class DraggableView : IDraggable, IDisposable
    {
        private readonly IDragData _dragData;
        private readonly RectTransform _transformToDrag;
        private readonly Action<DraggableView, DropResult> _onDragComplete;
        private readonly Func<DraggableView, DropResult, Sequence> _getDropAnimation;
        
        public RectTransform TransformToDrag => _transformToDrag;
        public IDragData DragData => _dragData;
        public Vector2 DragStartPosition { get; private set; }
        public Vector2 DragEndPosition { get; private set; }

        public DraggableView(IDragData dragData, 
            RectTransform transformToDrag, 
            Action<DraggableView, DropResult> onDragComplete = null,
            Func<DraggableView, DropResult, Sequence> getDropAnimation = null)
        {
            _dragData = dragData;
            _transformToDrag = transformToDrag;
            _onDragComplete = onDragComplete;
            _getDropAnimation = getDropAnimation;
        }

        public void OnDragStart()
        {
            DragStartPosition = _transformToDrag.position;
            _transformToDrag.gameObject.SetActive(true);
            _transformToDrag.localScale = Vector3.one;
        }

        public void DoSuccessDropAnimation(Action onComplete)
        {
            DragEndPosition = _transformToDrag.position;
            
            if (_getDropAnimation == null)
            {
                OnDropAnimationComplete(onComplete);
                return;
            }
            var dropAnimation = _getDropAnimation.Invoke(this, DropResult.Success);
            if (dropAnimation == null)
            {
                OnDropAnimationComplete(onComplete);
                return;
            }
            
            dropAnimation
                .Play()
                .OnComplete(() => OnDropAnimationComplete(onComplete));
        }
        
        public void DragComplete(DropResult dropResult)
        {
            DragEndPosition = _transformToDrag.position;
            _dragData.SendCallback(dropResult);
            _onDragComplete?.Invoke(this, dropResult);
            Dispose();
        }

        private void OnDropAnimationComplete(Action onHandleComplete)
        {
            onHandleComplete?.Invoke();
            _transformToDrag.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            Object.Destroy(TransformToDrag.gameObject);
        }
    }
}