using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Core.UI.Logs
{
    public class LogView : MonoBehaviour, ILogView
    {
        [SerializeField] private float _secondToHide;
        [SerializeField] private float _secondsToFade;
        [SerializeField] private TMP_Text _logText;
        private CompositeDisposable _compositeDisposable;

        public void ShowNewLog(string log)
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = new CompositeDisposable();
            
            _logText.gameObject.SetActive(true);
            _logText.text = log;
            AnimateNewText();
            
            Observable.Timer(TimeSpan.FromSeconds(_secondToHide))
                .Subscribe(l =>
                _logText.DOFade(0,_secondsToFade))
                .AddTo(_compositeDisposable);
        }

        private void AnimateNewText()
        {
            _logText.DOKill();
            _logText.transform.DOKill();

            var startTransform =  Vector3.one;
            _logText.DOFade(1,0);
            _logText.transform
                .DOScale(startTransform * 1.05f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .OnKill(() =>  _logText.transform.localScale = startTransform);
        }
    }
}