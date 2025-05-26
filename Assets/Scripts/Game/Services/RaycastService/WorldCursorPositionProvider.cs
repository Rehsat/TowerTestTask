using Game.Services.Cameras;
using Game.Services.Input;
using UnityEngine;

namespace Game.Services.RaycastService
{
    public class WorldCursorPositionProvider : IWorldCursorPositionProvider
    {
        private readonly ICameraService _cameraService;
        private readonly IInputService _inputService;
        
        public Vector3 WorldCursorPosition => GetWorldCursorPosition();

        public WorldCursorPositionProvider(ICameraService cameraService, IInputService inputService)
        {
            _cameraService = cameraService;
            _inputService = inputService;
        }

        public Vector3 GetWorldCursorPosition()
        {
            var currentCursorPosition = _inputService.PointerPosition;
            var worldPosition = _cameraService.MainCamera.ScreenToWorldPoint(currentCursorPosition);
            return worldPosition;
        }
    }
}