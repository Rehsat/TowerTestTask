using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public interface ITowerView : IDropHandler
    {
        public void SetLastViewTransform(Transform transform);
        public void PlaceFirstViewTransform(Transform transform);
    }

    public interface IDropHandler
    {
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject { get; }
    }
}