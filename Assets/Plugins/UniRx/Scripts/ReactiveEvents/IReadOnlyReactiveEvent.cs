using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyFramework.ReactiveEvents
{
    public interface IReadOnlyReactiveEvent<T>
    {
        T Value { get; }
        IDisposable Subscribe(Action<T> action);
        IDisposable SubscribeWithSkip(Action<T> action);
        Task<T> WaitNextEvent(CancellationToken cancellationToken);
    }
}