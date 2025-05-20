using Game.Services.Cameras;
using UnityEngine;
namespace Game.Services.OutOfScreenCheck
{
    public class OutOfScreenCheckService : IOutOfScreenCheckService
    {
        private readonly ICameraService _cameraService;
        private OutOfScreenCheckService(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public bool IsObjectOutOfScreen(Transform transformToCheck)
        {
            var mainCamera = _cameraService.MainCamera;
            if (mainCamera == null) return false;

            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transformToCheck.position);
            bool isOutside = viewportPosition.x < 0 || viewportPosition.x > 1 ||
                             viewportPosition.y < 0 || viewportPosition.y > 1 ||
                             viewportPosition.z < 0;
        
            return isOutside;
        }
    }
}
