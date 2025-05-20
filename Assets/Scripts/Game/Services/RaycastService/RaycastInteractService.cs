using Game.Core;
using Game.Services.Input;
using UnityEngine;

namespace Game.Services.RaycastService
{
    public class InteractService : IInteractService
    {
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;

        public InteractService(IInputService inputService, IRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            
            _inputService.OnDragActionStateChanged.SubscribeWithSkip((state =>
            {
                if (state == ActionState.Started)
                    Interact(inputService.PointerPosition);
            }));
        }

        public void Interact(Vector2 interactPosition)
        {
            if (_raycastService.TryGetComponentInRaycastHit<ITouchDetector>(interactPosition, out var touchDetector))
                touchDetector.Interact();
        }
    }
}