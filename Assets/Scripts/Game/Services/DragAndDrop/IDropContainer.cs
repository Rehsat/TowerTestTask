namespace Game.Services.DragAndDrop
{
    public interface IDropContainer
    {
        public void OnDrop(IDraggable droppedObject);
    }
}