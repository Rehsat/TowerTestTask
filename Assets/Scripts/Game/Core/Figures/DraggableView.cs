using System;
using Game.Services.DragAndDrop;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Figures
{
    public class DraggableView : IDraggable
    {
        private readonly IDragData _dragData;
        private readonly RectTransform _transformToDrag;
        private readonly Action<Vector3, DropResult> _onDragComplete;
        private readonly ParticleSystem _failParticle;
        public RectTransform TransformToDrag => _transformToDrag;
        public IDragData DragData => _dragData;

        public DraggableView(IDragData dragData, 
            RectTransform transformToDrag, 
            Action<Vector3, DropResult> onDragComplete = null)
        {
            _dragData = dragData;
            _transformToDrag = transformToDrag;
            _onDragComplete = onDragComplete;
        }
        public void OnDragStart()
        {
            _transformToDrag.gameObject.SetActive(true);
            _transformToDrag.localScale = Vector3.one;
        }

        public void OnDragComplete(DropResult dropResult)
        {
            _transformToDrag.gameObject.SetActive(false);
            _dragData.SendCallback(dropResult);
            _onDragComplete?.Invoke(_transformToDrag.position, dropResult);
        }
    }
}