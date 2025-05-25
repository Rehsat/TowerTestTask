using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Screen
{
    public class ByWidthAspectScreenSizeSpacingChanger : BaseScreenSizeChangeable
    {
        [SerializeField] private float _byAspectChangeMultiplier = 100;
        [SerializeField] private HorizontalLayoutGroup _layoutGroupToChangeSpacing;
        private float? _startSpacing;
        public override void ChangeByScreenSize(ScreenSizeContext sizeContext)
        {
            if (_startSpacing == null) _startSpacing = _layoutGroupToChangeSpacing.spacing;
            
            _layoutGroupToChangeSpacing.spacing =
                (float) _startSpacing + sizeContext.WidthAspectChange * _byAspectChangeMultiplier;
        }
    }
}