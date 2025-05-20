using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;
using Zenject;

namespace Game.Services.Canvases
{
    public class SceneCanvasesSetter : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<CanvasLayer, Canvas> _sceneCanvases;
        [Inject]
        public void Construct(ICanvasLayersProvider canvasLayersProvider)
        {
            foreach (var sceneCanvasesKey in _sceneCanvases.Keys)
            {
                var canvas = _sceneCanvases[sceneCanvasesKey];
                canvasLayersProvider.SetCanvasLayer(sceneCanvasesKey, canvas);
            }
        }
    }
}
