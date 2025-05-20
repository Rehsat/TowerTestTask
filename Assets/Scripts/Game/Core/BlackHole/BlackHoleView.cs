using System;
using DG.Tweening;
using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
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
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject => _dropContainer.OnObjectDropped;
        public void DoSuckAnimation(Transform transformToAnimate, Action onComplete = null)
        {
            var startEulerAngles = transformToAnimate.eulerAngles;
            startEulerAngles = new Vector3(startEulerAngles.x, startEulerAngles.y, _startRotation);

            transformToAnimate.localScale *= 0.65f;
            transformToAnimate.eulerAngles = startEulerAngles;
            transformToAnimate.position = _startSuckPosition.position;
            transformToAnimate
                .DOMove(_endPosition.position, _secondsToSuck)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
        }

    }
}
