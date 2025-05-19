using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public interface ITowerView
    {
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject { get; }
        public void SetLastViewTransform(Transform transform);
        public void PlaceFirstViewTransform(Transform transform);
    }
}