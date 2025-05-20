using Game.Core.Figures;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using UnityEngine;
using Zenject;

namespace Game.Services.DragAndDrop
{
    public class DragDataHandleService : IDragDataHandleService
    {
        private readonly IFactory<FigureData, FigureUI> _figureUiFactory;
        private readonly IDragService _dragService;

        public DragDataHandleService(
            IFactory<FigureData, FigureUI> figureUiFactory,
            IDragService dragService)
        {
            _figureUiFactory = figureUiFactory;
            _dragService = dragService;
        }
        public void HandleDragData(IDragData dragData)
        {
            //тут может быть словарь с разными классами/методами для хендла различных дат, но у меня она только одна, поэтому пока так
            if (dragData is DragFigureData dragFigureData)
                HandleFigureDragData(dragFigureData);
            else
                dragData.SendCallback(DropResult.Fail);
        }

        private void HandleFigureDragData(DragFigureData dragFigureData)
        {
            var figureUI = _figureUiFactory.Create(dragFigureData.FigureData);
            var figureRect = figureUI.GetComponent<RectTransform>();
            if (figureRect == null)
            {
                dragFigureData.SendCallback(DropResult.Fail);
                return;
            }
                
            var dragView = new DraggableView(dragFigureData, figureRect, null);
            _dragService.StartDrag(dragView);
        }
    }
}