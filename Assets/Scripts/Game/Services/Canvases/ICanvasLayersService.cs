using UnityEngine;

namespace Game.Services.Canvases
{
    public interface ICanvasLayersService
    {
        public Canvas GetCanvasByLayer(CanvasLayer canvasLayer);
    }
}