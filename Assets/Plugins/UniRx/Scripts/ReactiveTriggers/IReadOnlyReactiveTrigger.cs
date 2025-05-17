using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyFramework.ReactiveTriggers
{
    public interface IReadOnlyReactiveTrigger
    {
        IDisposable Subscribe(Action action);
        IDisposable SubscribeWithSkip(Action action);
        Task WaitNextTriggered(CancellationToken cancellationToken);
    }
}