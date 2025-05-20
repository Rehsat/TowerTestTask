using System;
using Game.Services.LogService;
using UniRx;

namespace Game.Core.UI.Logs
{
    public class LogsPresenter : IDisposable
    {
        private CompositeDisposable _compositeDisposable;
        public LogsPresenter(ILogService logService, ILogView logView)
        {
            _compositeDisposable = new CompositeDisposable();
            logService.OnNewLoggedString
                .SubscribeWithSkip(logView.ShowNewLog)
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}