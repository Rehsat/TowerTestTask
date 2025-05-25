using System;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.Tower;
using UniRx;

namespace Game.Services.LogService.Loggers
{
    public class TowerBuildLogger : ILogsCreator, IDisposable
    {
        private CompositeDisposable _compositeDisposable;
        private ReactiveEvent<LocalizableLogData> _onNewLogs;
        public IReadOnlyReactiveEvent<LocalizableLogData> OnNewLogs => _onNewLogs;

        public TowerBuildLogger(TowerPresenter towerPresenter)
        {
            _onNewLogs = new ReactiveEvent<LocalizableLogData>();
            _compositeDisposable = new CompositeDisposable();
        
            towerPresenter.OnNewFigureAdded
                .SubscribeWithSkip(LogNewFigureAdded)
                .AddTo(_compositeDisposable);
            
            towerPresenter.OnFigureOutOfRange
                .SubscribeWithSkip(LogFigureOutOfRange)
                .AddTo(_compositeDisposable);

            towerPresenter.OnFigureRemoved
                .SubscribeWithSkip(LogFigureRemove)
                .AddTo(_compositeDisposable);
        }
    
        private void LogFigureRemove(int index)
        {
            var listOfLogStrings = new List<string>()
            {
                "Фигура под номером",
                index.ToString(),
                "была убрана"
            };
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }

        private void LogFigureOutOfRange()
        {
            var listOfLogStrings = new List<string>(){ "Фигура не помещается в границах экрана" };
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }

        private void LogNewFigureAdded(FigureData figureData)
        {
            var listOfLogStrings = new List<string>(){"Была поставлена новая фиугра"};
            _onNewLogs.Notify(new LocalizableLogData(listOfLogStrings));
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onNewLogs?.Dispose();
        }
    }
}
