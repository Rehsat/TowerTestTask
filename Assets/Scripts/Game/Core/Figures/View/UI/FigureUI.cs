using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.Figures.UI
{
    public class FigureUI : MonoBehaviour, IPointerDownHandler, IFigureInteractableView
    {
        [SerializeField] private Image _icon;
        
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<FigureData> _onFigureInteracted;
        private FigureData _figureData;

        public FigureData FigureData => _figureData;
        public void SetSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void SetInteractableData(FigureData figureData, ReactiveEvent<FigureData> onInteract)
        {
            _figureData = figureData;
            _onFigureInteracted = onInteract;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_figureData == null)
            {
                var errorObjectId = $"{gameObject.name}{GetHashCode()}";
                gameObject.name = errorObjectId;
                Debug.LogError($"There is no data on figure view with id {errorObjectId}");
                return;
            }
            _onFigureInteracted.Notify(_figureData);
        }
    }
}
