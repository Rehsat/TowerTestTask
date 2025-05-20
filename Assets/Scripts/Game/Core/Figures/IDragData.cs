using Game.Services.DragAndDrop;

namespace Game.Core.Figures
{
    public interface IDragData
    {
        public void SendCallback(DropResult dropResult);
    }
}