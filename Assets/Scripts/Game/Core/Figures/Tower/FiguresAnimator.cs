using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public class FiguresAnimator
    {
        private Sequence _figuresAnimationSequence;
        private bool _isEnabled = false;
        private Dictionary<int, Sequence> _sequences = new Dictionary<int, Sequence>();
        
        public void DoJumpAnimation(Transform transformToAnimate)
        {
            if (_isEnabled == false) return;
            
            ResetSequence(transformToAnimate);
            transformToAnimate.localScale = Vector3.zero;
            var startPosition = transformToAnimate.localPosition;
            
            var jumpTween =
                transformToAnimate
                    .DOLocalMoveY(startPosition.y + 1, 0.5f)
                    .SetEase(Ease.OutCubic);
            
            var returnTween = 
                transformToAnimate
                    .DOLocalMoveY(startPosition.y, 0.4f)
                    .SetEase(Ease.InCubic);
            
            var appearTween = GetAppearAnimation(transformToAnimate);

            _figuresAnimationSequence
                .Append(jumpTween)
                .Join(appearTween)
                .Append(returnTween);
            
            _figuresAnimationSequence.Play();
        }
        
        public void DoDropAnimation(Transform transformToAnimate, Vector2 resultPosition, bool fastMode = false)
        {
            if(_isEnabled == false) return;
            
            ResetSequence(transformToAnimate);
            var speed = fastMode ? 2 : 1;
            var ease = fastMode ? Ease.Linear : Ease.InBack;
            var dropTween = transformToAnimate
                .DOLocalMoveY(resultPosition.y, 0.6f / speed)
                .SetEase(ease);

            _figuresAnimationSequence
                .Append(dropTween)
                .OnComplete(() =>
                {
                    var tolerance = 0.1f;
                    if (Math.Abs(transformToAnimate.localPosition.y - resultPosition.y) > tolerance)
                        DoDropAnimation(transformToAnimate, resultPosition, true);
                });

            _figuresAnimationSequence.Play();
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        private Tween GetAppearAnimation(Transform transformToAnimate)
        {
            var resultScale = Vector3.one;
            transformToAnimate.localScale = Vector3.zero;
            return transformToAnimate
                .DOScale(resultScale, 0.3f)
                .SetEase(Ease.OutBack)
                .OnKill(() =>
                    transformToAnimate.localScale = resultScale);
        }
        
        private void ResetSequence(Transform transformToAnimate)
        {
            var key = transformToAnimate.GetInstanceID();
            if (_sequences.ContainsKey(key))
            {
                _figuresAnimationSequence = _sequences[key];
                _figuresAnimationSequence?.Kill();
                _figuresAnimationSequence = DOTween.Sequence();
                _sequences[key] = _figuresAnimationSequence;
                return;
            }
            _figuresAnimationSequence = DOTween.Sequence();
            _sequences.Add(key, _figuresAnimationSequence);
        }
    }
}