using System;
using System.Collections.Generic;
using Game.Infrastructure;
using UniRx;
using UnityEngine;

namespace Game.Core.Screen
{
    public class ScreenSizeChangeableRoot : MonoBehaviour
    {
        [SerializeField] private List<BaseScreenSizeChangeable> _changeables;

#if UNITY_EDITOR
        private void Awake()
        {
            Observable.EveryUpdate().Subscribe(f=>RecalculateByScreenSize());
        }
#endif
        public void OnEnable()
        {
            RecalculateByScreenSize();
        }

        private void RecalculateByScreenSize()
        {
            var screenSizeContext = GetNewScreenSizeContext();
            _changeables.ForEach(changeable => changeable.ChangeByScreenSize(screenSizeContext));
        }

        private ScreenSizeContext GetNewScreenSizeContext()
        {
            var referenceScreenSize =
                new Vector2(GameConstants.ReferenceScreenWidth, GameConstants.ReferenceScreenHeight);
            var currentScreenSize = 
                new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);

            var widthAspectChange = GetWidthAspectChange(referenceScreenSize, currentScreenSize);
            
            return new ScreenSizeContext(widthAspectChange);
        }

        private float GetWidthAspectChange(Vector2 referenceScreen, Vector2 currentScreen)
        {
            var referenceWidthAspect = referenceScreen.x / referenceScreen.y;
            var currentScreenAspect = currentScreen.x / currentScreen.y;
            
            return referenceWidthAspect - currentScreenAspect;
        }
    }

    public class ScreenSizeContext
    {
        public float WidthAspectChange { get; }

        public ScreenSizeContext(float widthAspectChange)
        {
            WidthAspectChange = widthAspectChange;
        }
    }
}
