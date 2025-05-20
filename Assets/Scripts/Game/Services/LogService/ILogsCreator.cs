using EasyFramework.ReactiveEvents;

namespace Game.Services.LogService
{
    public interface ILogsCreator
    {
        public IReadOnlyReactiveEvent<LocalizableLogData> OnNewLogs { get; }
    }
}