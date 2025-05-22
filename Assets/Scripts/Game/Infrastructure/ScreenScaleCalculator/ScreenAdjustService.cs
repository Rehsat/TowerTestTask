using EasyFramework.ReactiveTriggers;
using Game.Services.Cameras;
using UniRx;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Game.Infrastructure.ScreenScale
{
    public class ScreenAdjustService : IScreenScaleCalculator
    {
        private readonly ICameraService _cameraService;
        private float _startAspectRatio;
        private ReactiveTrigger _onScreenSizeChange;
        public float CameraSizeChange { get; private set; }
        public Vector2 ScreenSize { get; private set; }
        public Vector2 SpritesTransformPositionChange { get; private set; }
        public IReadOnlyReactiveTrigger OnChangeScreenSize => _onScreenSizeChange;

        public ScreenAdjustService(ICameraService cameraService)
        {
            _cameraService = cameraService;
            _onScreenSizeChange = new ReactiveTrigger();
            _startAspectRatio = cameraService.MainCamera.orthographicSize;
            CalculateDatasByScreenSize();
#if UNITY_EDITOR
            Observable.IntervalFrame(1).Subscribe((l =>
            {
                CalculateDatasByScreenSize();
                _onScreenSizeChange.Notify();
            }));
#endif
        }

        private void CalculateDatasByScreenSize()
        {
            ScreenSize = new Vector2(Screen.width, Screen.height);
            var screenAspectRatio = ScreenSize.x/ScreenSize.y;
            var referenceAspectRatio = GameConstants.ReferenceScreenSize.x / GameConstants.ReferenceScreenSize.y;
            var difference = Mathf.Abs(screenAspectRatio - referenceAspectRatio);
            /*if (screenAspectRatio >= referenceWidth)
            {
                var widthRatio = screenAspectRatio.GetRatio(referenceWidth, true);
                CameraSizeChange =  widthRatio * -0.7f;
            }
            else
            {
                var widthRatio = referenceWidth.GetRatio(screenAspectRatio);
                
            }
            Debug.LogError(CameraSizeChange);*/
            CameraSizeChange = -difference * 2.5f;
            _cameraService.MainCamera.orthographicSize = _startAspectRatio + CameraSizeChange;
        }
    }
}