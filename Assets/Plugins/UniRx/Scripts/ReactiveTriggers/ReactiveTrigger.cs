using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;

namespace EasyFramework.ReactiveTriggers
{
    public class ReactiveTrigger : IReadOnlyReactiveTrigger, IWriteOnlyReactiveTrigger, IDisposable
    {
        private readonly BoolReactiveProperty _boolReactiveProperty = new(true);
        
        public IDisposable Subscribe(Action action)
        {
            return _boolReactiveProperty.Subscribe(onNext => action());
        }

        public IDisposable SubscribeWithSkip(Action action)
        {
            return _boolReactiveProperty.Skip(1).Subscribe(onNext => action());
        }

        public async Task WaitNextTriggered(CancellationToken cancellationToken)
        {
            var triggered = false;

            void Trigger() => triggered = true;

            var subscribe= SubscribeWithSkip(Trigger);
            while (triggered is false && cancellationToken.IsCancellationRequested is false)
            {
                await Task.Yield();
            }
            subscribe.Dispose();
        }

        public void Notify()
        {
            _boolReactiveProperty.SetValueAndForceNotify(true);
        }

        public void Dispose()
        {
            _boolReactiveProperty?.Dispose();
        }
    }
}