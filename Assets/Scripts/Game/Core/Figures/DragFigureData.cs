using System;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures
{
    public class DragFigureData : IDragData
    {
        private readonly Action<DropResult> _onComplete;
        public FigureData FigureData { get; }

        public DragFigureData(FigureData figureData, Action<DropResult> onComplete)
        {
            _onComplete = onComplete;
            FigureData = figureData;
        }

        public void SendCallback(DropResult dropResult)
        {
            _onComplete?.Invoke(dropResult);
        }
    }

    public interface IDragData
    {
        public void SendCallback(DropResult dropResult);
    }


    public class DraggableView : IDraggable
    {
        private readonly IDragData _dragFigureData;
        private readonly RectTransform _transformToDrag;
        private readonly ParticleSystem _failParticle;
        public RectTransform TransformToDrag => _transformToDrag;
        public IDragData DragFigureData => _dragFigureData;

        public DraggableView(IDragData dragFigureData, RectTransform transformToDrag, ParticleSystem failParticle)
        {
            _dragFigureData = dragFigureData;
            _transformToDrag = transformToDrag;
            _failParticle = failParticle;
        }
        public void OnDragStart()
        {
            _transformToDrag.gameObject.SetActive(true);
        }

        public void OnDragComplete(DropResult dropResult)
        {
            if (dropResult == DropResult.Fail)
            {
                //_failParticle.transform.position = _transformToDrag.transform.position;
            }
            _transformToDrag.gameObject.SetActive(false);
            _dragFigureData.SendCallback(dropResult);
        }
    }
}
