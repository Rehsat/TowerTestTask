using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public class FiguresTowerBuilder : ITowerBuilder
    {
        private readonly FiguresAnimator _figuresAnimator;
        private readonly ITowerView _towerView;
        private List<Transform> _towerElements;

        public FiguresTowerBuilder(FiguresAnimator figuresAnimator, ITowerView towerView)
        {
            _figuresAnimator = figuresAnimator;
            _towerView = towerView;
            _towerElements = new List<Transform>();
        }

        public void PlaceNewElementInTower(Transform elementTransform, Vector2 offset)
        {
            if (_towerElements.Count > 0)
                ConnectViewToLast(offset, elementTransform);
            else
                _towerView.PlaceFirstViewTransform(elementTransform);

            _towerView.SetLastViewTransform(elementTransform);
            _towerElements.Add(elementTransform);
            _figuresAnimator.DoJumpAnimation(elementTransform);
        }

        public void RemoveFromTower(int index)
        {
            if (IsIndexValid(index) == false)
                return;

            _figuresAnimator.KillCurrentAnimation();
            var isLastElement = _towerElements.Count - 1 == index;

            RemoveElementViewFromTower(index, isLastElement);
            _towerElements.RemoveAt(index);
        }

        private void ConnectViewToLast(Vector2 offsetPercent, Transform figureSpriteView)
        {
            var currentLastView = _towerElements[^1];
            var xPosition = currentLastView.localScale.x * (offsetPercent.x / 100f);

            figureSpriteView.parent = currentLastView;
            figureSpriteView.localPosition =
                new Vector2(xPosition, currentLastView.localScale.y) * 2;
        }

        private bool IsIndexValid(int index)
        {
            if (_towerElements.Count > index)
                return true;

            Debug.LogError("List of views in tower was corrupted");
            return false;
        }

        private void RemoveElementViewFromTower(int index, bool isLastElement)
        {
            var viewToRemove = _towerElements[index];
            if (isLastElement)
                HandleLastFigureRemoval(index);
            else
                HandleNonLastFigureRemoval(index, viewToRemove);
        }

        private void HandleLastFigureRemoval(int index)
        {
            Transform newLastFigure = index > 0
                ? _towerElements[index - 1]
                : null;

            _towerView.SetLastViewTransform(newLastFigure);
        }

        private void HandleNonLastFigureRemoval(int index, Transform viewToRemove)
        {
            var nextViewInTower = _towerElements[index + 1];
            nextViewInTower.parent = viewToRemove.parent;

            var nextViewPosition = nextViewInTower.localPosition;
            var newNextViewPosition =
                new Vector2(nextViewPosition.x, nextViewPosition.y - viewToRemove.localScale.y * 2);
            _figuresAnimator.DoDropAnimation(nextViewInTower, newNextViewPosition);

        }
    }
}