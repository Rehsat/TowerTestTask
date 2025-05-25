using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public interface ITowerBuilder
    {
        public void RemoveFromTower(int index);
        public void PlaceNewElementInTower(Transform elementTransform, Vector2 offset);
    }
}