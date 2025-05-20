using System;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Services.DragAndDrop;

namespace Game.Core.Figures
{
    public class DragFigureData : IDragData
    {
        private readonly Action<DropResult> _onComplete;
        
        private FigureData _figureData;
        public FigureData FigureData => _figureData;

        public DragFigureData(FigureData figureData, Action<DropResult> onComplete)
        {
            _figureData = figureData;
            _onComplete = onComplete;
        }

        public void SendCallback(DropResult dropResult)
        {
            _onComplete?.Invoke(dropResult);
        }

        public void SetNewFigureData(FigureData figureData)
        {
            _figureData = figureData;
        }
    }
}
