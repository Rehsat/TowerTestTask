using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.BlackHole;
using Game.Core.Figures.Tower;
using UniRx;

namespace Game.Services.LogService.Loggers
{
    public class BlackHoleLogger : ILogsCreator
    {
        private readonly BlackHolePresenter _blackHolePresenter;
        private ReactiveEvent<LocalizableLogData> _onNewLogs;
        public IReadOnlyReactiveEvent<LocalizableLogData> OnNewLogs => _onNewLogs;

        public BlackHoleLogger(BlackHolePresenter blackHolePresenter)
        {
            _blackHolePresenter = blackHolePresenter;
            _onNewLogs = new ReactiveEvent<LocalizableLogData>();
            
            _blackHolePresenter.OnFigureSucked.SubscribeWithSkip(f => LogFigureDestroyed());
        }
        private void LogFigureDestroyed()
        {
            var listOfLogStrings = new List<string>(){"Фигура умерла страшной смертью в пасти черной дыры. Ты ужасен!"};
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }
    }
}