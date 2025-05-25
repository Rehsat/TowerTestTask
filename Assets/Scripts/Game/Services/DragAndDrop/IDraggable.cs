using System;
using UnityEngine;

namespace Game.Services.DragAndDrop
{
    public interface IDraggable
    {
        public RectTransform TransformToDrag { get; }
        public void OnDragStart();
        public void DoSuccessDropAnimation(Action onComplete);
        public void DragComplete(DropResult dropResult);
    }
}