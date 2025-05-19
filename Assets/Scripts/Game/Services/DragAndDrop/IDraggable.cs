using UnityEngine;

namespace Game.Services.DragAndDrop
{
    public interface IDraggable
    {
        public RectTransform TransformToDrag { get; }
        public void OnDragStart();
        public void OnDragComplete(DropResult dropResult);
    }
}