using System.Collections.ObjectModel;
using EasyFramework.ReactiveEvents;

namespace Game.Services.LogService
{
    public interface ILogService
    {
        public IReadOnlyReactiveEvent<string> OnNewLoggedString { get; }
        public void OnNewLogData(LocalizableLogData localizableLogData);
    }
}
