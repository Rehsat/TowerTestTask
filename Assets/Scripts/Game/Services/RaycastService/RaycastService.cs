using Game.Services.Cameras;
using UnityEngine;

namespace Game.Services.RaycastService
{
    public class RaycastService : IRaycastService
    {
        private readonly ICameraService _cameraService;

        public RaycastService(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }
        
        public RaycastHit2D DoRaycast(Vector2 screenPosition)
        {
            Ray ray = _cameraService.MainCamera.ScreenPointToRay(screenPosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            return hit;
        }

        public bool TryGetComponentInRaycastHit<TComponent>(Vector2 screenPosition, out TComponent component)
        {
            var hit = DoRaycast(screenPosition);
            
            if (hit.collider != null &&
                hit.collider.TryGetComponent<TComponent>(out component))
                return true;

            component = default;
            return false;
        }
    }
}