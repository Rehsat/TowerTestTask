using Game.Core.Figures;

namespace Game.Services.DragAndDrop
{
    public interface IDragDataHandleService
    {
        public void HandleDragData(IDragData dragData);
    }
}