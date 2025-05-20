using Game.Infrastructure.AssetsManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Factories
{
    public class CanvasFactory : IFactory<Canvas>
    {
        private Canvas _canvasPrefab;
        private const float REFERENCED_RESOLUTION_X = 1920;
        private const float REFERENCED_RESOLUTION_Y = 1080;
        
        public CanvasFactory(IPrefabsProvider prefabsProvider)
        {
            _canvasPrefab = prefabsProvider.GetPrefabsComponent<Canvas>(Prefab.Canvas);
        }
        public Canvas Create()
        {
            var instantinatedCanvas = Object.Instantiate(_canvasPrefab);
            if (instantinatedCanvas.TryGetComponent<CanvasScaler>(out var canvasScaler))
            {
                var referenceResolution = new Vector2(REFERENCED_RESOLUTION_X, REFERENCED_RESOLUTION_Y);
                canvasScaler.referenceResolution = referenceResolution;
            }

            return instantinatedCanvas;
        }
    }
}
