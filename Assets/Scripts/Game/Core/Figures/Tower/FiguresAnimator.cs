using DG.Tweening;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public class FiguresAnimator
    {
        private Sequence _figuresAnimationSequence;
        
        public void DoJumpAnimation(Transform transformToAnimate)
        {
            ResetSequence();
            
            transformToAnimate.localScale = Vector3.zero;
            var startPosition = transformToAnimate.position;
            
            var jumpTween =
                transformToAnimate
                    .DOMoveY(transformToAnimate.position.y + 1, 0.5f)
                    
                    .SetEase(Ease.OutCubic);
            var returnTween = 
                transformToAnimate
                    .DOMove(startPosition, 0.4f)
                    .SetEase(Ease.InCubic);
            
            var appearTween = GetAppearAnimation(transformToAnimate);
            
            _figuresAnimationSequence
                .Append(jumpTween)
                .Join(appearTween)
                .Append(returnTween)
                .OnKill((() => transformToAnimate.position = startPosition));
            _figuresAnimationSequence.Play();
        }
        public void DoDropAnimation(Transform transformToAnimate, Vector2 resultPosition)
        {
            ResetSequence();

            var dropTween = transformToAnimate
                .DOLocalMove(resultPosition, 0.6f)
                .SetEase(Ease.InBack);
            
            _figuresAnimationSequence
                .Append(dropTween)
                .OnKill(() => transformToAnimate.localPosition = resultPosition);

            _figuresAnimationSequence.Play();
        }

        private Tween GetAppearAnimation(Transform transformToAnimate)
        {
            var resultScale = Vector3.one;
            return transformToAnimate
                .DOScale(resultScale, 0.3f)
                .SetEase(Ease.OutBack)
                .OnKill(() => transformToAnimate.localScale = resultScale);
        }

        private void ResetSequence()
        {
            _figuresAnimationSequence?.Kill();
            _figuresAnimationSequence = DOTween.Sequence();
        }

        public void KillCurrentAnimation()
        {
            _figuresAnimationSequence?.Kill();
        }
    }
}