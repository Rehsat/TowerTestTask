using UnityEngine;

namespace Game.Services.Canvases
{
    public interface ICanvasLayersProvider
    {
        public Canvas GetCanvasByLayer(CanvasLayer canvasLayer);
    }
}