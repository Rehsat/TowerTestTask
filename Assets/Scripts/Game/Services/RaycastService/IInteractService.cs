using UnityEngine;

namespace Game.Services.RaycastService
{
    public interface IInteractService
    {
        public void Interact(Vector2 interactPosition);
    }
}