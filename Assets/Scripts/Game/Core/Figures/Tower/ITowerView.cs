using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public interface ITowerView : IDropHandler
    {
        public Transform DropContainerTransform { get; }
        public void SetLastViewTransform(Transform transform);
        public void PlaceFirstViewTransform(Transform transform);
    }
}