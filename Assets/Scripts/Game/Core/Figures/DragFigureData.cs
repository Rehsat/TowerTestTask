using System;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Services.DragAndDrop;

namespace Game.Core.Figures
{
    public class DragFigureData : IDragData
    {
        private readonly Action<DropResult> _onComplete;
        public DragFigureSource Source { get; }
        public FigureData FigureData { get; private set; }

        public DragFigureData(
            FigureData figureData, 
            Action<DropResult> onComplete, 
            DragFigureSource source)
        {
            FigureData = figureData;
            Source = source;
            _onComplete = onComplete;
        }

        public void SendCallback(DropResult dropResult)
        {
            _onComplete?.Invoke(dropResult);
        }
    }
}
