using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;

namespace Game.Core.Figures.UI
{
    public interface IFigureInteractableView : IFigureView
    {
        public void SetInteractableData(FigureData figureData, ReactiveEvent<FigureData> onInteract);
    }
}