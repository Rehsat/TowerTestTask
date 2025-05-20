using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using UniRx;
using UnityEngine;

namespace Game.Core.Figures.View
{
    public class FigureSpriteView : MonoBehaviour, IFigureInteractableView, IDisposable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PlayerTouchDetector _touchDetector;
        
        private FigureData _figureData;
        private ReactiveEvent<FigureData> _onInteract;
        private CompositeDisposable _compositeDisposable;
        private Tween _placeTween;
        private bool _touchDetectorWasInitialized;
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void SetInteractableData(FigureData figureData, ReactiveEvent<FigureData> onInteract)
        {
            if (_touchDetectorWasInitialized == false)
                InitializeTouchDetector();
            
            _figureData = figureData;
            _onInteract = onInteract;
        }
        
        public void SetMaskMode(SpriteMaskInteraction spriteMaskInteraction)
        {
            _spriteRenderer.maskInteraction = spriteMaskInteraction;
        }

        public void ReturnToPool() // т.к. их не особо много смысла в пуле нет, но если понадобится, то метод уже в нужных местах вызван
        {
            Dispose();
            Destroy(gameObject);
        }
        private void InitializeTouchDetector()
        {
            _compositeDisposable = new CompositeDisposable();
            
            _touchDetector.Construct();
            _touchDetector.OnTouch
                .SubscribeWithSkip(OnTouch)
                .AddTo(_compositeDisposable);
            
            _touchDetectorWasInitialized = true;
        }

        private void OnTouch()
        {
            if (_figureData == null || _onInteract == null)
            {
                var id = $"{gameObject.name}{GetHashCode()}";
                gameObject.name = id;
                Debug.LogError($"InteractableData {id} was not implemented");
                return;
            }
            _onInteract.Notify(_figureData);
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer.GetComponent<SpriteRenderer>();
            if (_touchDetector == null)
                _spriteRenderer.GetComponent<PlayerTouchDetector>();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
