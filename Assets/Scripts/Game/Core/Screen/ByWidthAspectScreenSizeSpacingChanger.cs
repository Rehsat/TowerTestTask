using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Screen
{
    public class ByWidthAspectScreenSizeSpacingChanger: BaseScreenSizeChangeable
    {
        [SerializeField] private float _byAspectChangeMultiplier;
        [SerializeField] private HorizontalLayoutGroup _layoutGroupToChange;
        private float? _startSpacing;
        public override void ChangeByScreenSize(ScreenSizeContext sizeContext)
        {
            if (_startSpacing == null) _startSpacing = _layoutGroupToChange.spacing;

            _layoutGroupToChange.spacing = (float) _startSpacing + _byAspectChangeMultiplier * sizeContext.WidthAspectChange;
        }
    }
}