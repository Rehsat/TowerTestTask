using UnityEngine;

namespace Game.Core.Screen
{
    public class ByWidthAspectScreenSizeScaler : BaseScreenSizeChangeable
    {
        [SerializeField] private float _byAspectChangeMultiplier;
        [SerializeField] private Transform _transformToScale;
        private Vector3? _startScale;
        public override void ChangeByScreenSize(ScreenSizeContext sizeContext)
        {
            if (_startScale == null) _startScale = _transformToScale.localScale;

            _transformToScale.localScale = (Vector3)_startScale + (Vector3.one * Mathf.Abs(sizeContext.WidthAspectChange) 
                                                                               * _byAspectChangeMultiplier);
        }
    }
}