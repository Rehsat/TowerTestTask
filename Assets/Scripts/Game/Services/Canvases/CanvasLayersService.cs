using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Services.Canvases
{
    public class CanvasLayersService : MonoBehaviour, ICanvasLayersService
    {
        [SerializeField] private Transform _canvasesRoot;
        [SerializeField] private List<CanvasLayer> _layersOrder;
        
        private IFactory<Canvas> _canvasFactory;
        private Dictionary<CanvasLayer, Canvas> _canvases;
        [Inject]
        public void Construct(IFactory<Canvas> canvasFactory)
        {
            _canvasFactory = canvasFactory;
            _canvases  = new Dictionary<CanvasLayer, Canvas>();
            for (var i = 0; i < _layersOrder.Count; i++)
            {
                var layer = _layersOrder[i];
                if (_canvases.ContainsKey(layer))
                {
                    Debug.LogError($"{layer} duplicates in _layersOrder");
                    continue;
                }
                    
                var canvas = _canvasFactory.Create();
                canvas.transform.parent = _canvasesRoot;
                canvas.sortingOrder = i;
                canvas.gameObject.name = layer.ToString();
                
                _canvases.Add(layer, canvas);
            }
        }

        public Canvas GetCanvasByLayer(CanvasLayer canvasLayer)
        {
            if (_canvases.ContainsKey(canvasLayer))
                return _canvases[canvasLayer];
            
            Debug.LogError($"There is no {canvasLayer} created. Add it in CanvasLayersService _layersOrder");
            return null;
        }
    }
}