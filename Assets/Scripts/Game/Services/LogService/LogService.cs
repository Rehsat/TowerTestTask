using System;
using System.Collections.Generic;
using System.Text;
using EasyFramework.ReactiveEvents;
using Game.Services.Localization;
using UniRx;

namespace Game.Services.LogService
{
    public class LogService : ILogService, IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private CompositeDisposable _compositeDisposable;
        
        private ReactiveEvent<string> _onNewLoggedString;
        
        public IReadOnlyReactiveEvent<string> OnNewLoggedString => _onNewLoggedString;

        public LogService(ILocalizationService localizationService, List<ILogsCreator> logsCreators)
        {
            _localizationService = localizationService;
            _onNewLoggedString = new ReactiveEvent<string>();
            _compositeDisposable = new CompositeDisposable();
            
            logsCreators.ForEach(creator => 
                creator.OnNewLogs
                    .SubscribeWithSkip(OnNewLogData)
                    .AddTo(_compositeDisposable));
        }

        //Можно будет выделить интерфейс ILogData и тут вместо логики определять подходящий метод для конкретной даты 
        public void OnNewLogData(LocalizableLogData localizableLogData)
        {
            var stringBuilder = new StringBuilder();
            localizableLogData.LocalizableStrings.ForEach(localizableString =>
            {
                stringBuilder.Append(_localizationService.Localize(localizableString));
                stringBuilder.Append(' ');
            });
            
            var resultString = stringBuilder.ToString();
            _onNewLoggedString.Notify(resultString);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _onNewLoggedString?.Dispose();
        }
    }
}