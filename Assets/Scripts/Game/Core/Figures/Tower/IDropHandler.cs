using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;

namespace Game.Core.Figures.Tower
{
    public interface IDropHandler
    {
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject { get; }
    }
}