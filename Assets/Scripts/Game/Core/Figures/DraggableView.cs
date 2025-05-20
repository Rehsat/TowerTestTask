using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures
{
    public class DraggableView : IDraggable
    {
        private readonly IDragData _dragData;
        private readonly RectTransform _transformToDrag;
        private readonly ParticleSystem _failParticle;
        public RectTransform TransformToDrag => _transformToDrag;
        public IDragData DragData => _dragData;

        public DraggableView(IDragData dragData, RectTransform transformToDrag, ParticleSystem failParticle)
        {
            _dragData = dragData;
            _transformToDrag = transformToDrag;
            _failParticle = failParticle;
        }
        public void OnDragStart()
        {
            _transformToDrag.gameObject.SetActive(true);
            _transformToDrag.localScale = Vector3.one;
        }

        public void OnDragComplete(DropResult dropResult)
        {
            if (dropResult == DropResult.Fail)
            {
                //_failParticle.transform.position = _transformToDrag.transform.position;
            }
            _transformToDrag.gameObject.SetActive(false);
            _dragData.SendCallback(dropResult);
        }
    }
}