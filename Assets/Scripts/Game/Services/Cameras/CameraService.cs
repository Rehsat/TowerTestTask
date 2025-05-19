using UnityEngine;

namespace Game.Services.Cameras
{
    // В обновлениях можно будет добавить управление камерами (переключение и т.п)
    public class CameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private Camera _mainCamera;
        public Camera MainCamera => _mainCamera;
    }
}