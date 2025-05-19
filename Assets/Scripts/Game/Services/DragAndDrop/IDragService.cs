using System;

namespace Game.Services.DragAndDrop
{
    public interface IDragService
    {
        public void StartDrag(IDraggable draggable);
    }
}
