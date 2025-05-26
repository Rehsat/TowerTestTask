using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
using Game.Services.RaycastService;
using UniRx;
using UnityEngine;

namespace Game.Core.BlackHole
{
    public class BlackHoleView : MonoBehaviour, IBlackHoleView
    {
        [SerializeField] private float _secondsToSuck;
        [SerializeField] private float _startRotation;
        [SerializeField] private DropCollider _dropContainer;
        [SerializeField] private Transform _startSuckPosition;
        [SerializeField] private Transform _endPosition;
        
        private IWorldCursorPositionProvider _worldCursorPositionProvider;
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject => _dropContainer.OnObjectDropped;

        public void Construct(IWorldCursorPositionProvider worldCursorPositionProvider)
        {
            _worldCursorPositionProvider = worldCursorPositionProvider;
        }
        public void DoSuckAnimation(Transform transformToAnimate, Action onComplete = null)
        {
            var startEulerAngles = transformToAnimate.eulerAngles;
            startEulerAngles = new Vector3(startEulerAngles.x, startEulerAngles.y, _startRotation);

            transformToAnimate.localScale *= 0.65f;
            transformToAnimate.position = _worldCursorPositionProvider.WorldCursorPosition;

            var sequence = DOTween.Sequence();

            var moveToStartTween = transformToAnimate
                .DOMove(_startSuckPosition.position, 0.3f)
                .SetEase(Ease.OutCubic);
            var rotateTween = transformToAnimate
                .DORotate(startEulerAngles, _secondsToSuck / 3)
                .SetEase(Ease.OutBack);
            var suckPositionTween = transformToAnimate
                .DOMove(_endPosition.position, _secondsToSuck)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
            
            sequence
                .Append(moveToStartTween)
                .Join(rotateTween)
                .Append(suckPositionTween);
            sequence.Play();
        }


    }
}
