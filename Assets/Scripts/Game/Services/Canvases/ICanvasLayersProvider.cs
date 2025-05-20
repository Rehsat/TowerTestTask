using UnityEngine;

namespace Game.Services.Canvases
{
    public interface ICanvasLayersProvider
    {
        public Canvas GetCanvasByLayer(CanvasLayer canvasLayer);
        public void SetCanvasLayer(CanvasLayer layer, Canvas canvas);
    }
}