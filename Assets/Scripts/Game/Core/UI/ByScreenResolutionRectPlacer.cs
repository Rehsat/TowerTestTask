using System;
using UniRx;
using UnityEngine;

namespace Game.Core.UI
{
    public class ByScreenResolutionRectPlacer : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _referenceResolution;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2 _anchoredPosition;

        private void OnEnable()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            var divide = _referenceResolution / _size;

            _rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                screenWidth / divide.x
            );
            _rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, 
                screenHeight / divide.y
            );
            
            _rectTransform.anchoredPosition = _anchoredPosition;
            Observable.TimerFrame(1).Subscribe((l =>
            {
                _rectTransform.anchoredPosition = _anchoredPosition;
            }));
        }
    }
}
