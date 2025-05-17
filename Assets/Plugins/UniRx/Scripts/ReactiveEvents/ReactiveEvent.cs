using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;

namespace EasyFramework.ReactiveEvents
{
    public class ReactiveEvent<T> : IReadOnlyReactiveEvent<T>, IDisposable
    {
        public T Value => _reactiveProperty.Value;
        
        private readonly ReactiveProperty<T> _reactiveProperty = new();
        
        public void Notify(T value)
        {
            _reactiveProperty.SetValueAndForceNotify(value);
        }
        
        public IDisposable Subscribe(Action<T> action)
        {
            return _reactiveProperty.Subscribe(action);
        }

        public IDisposable SubscribeWithSkip(Action<T> action)
        {
            return _reactiveProperty.Skip(1).Subscribe(action);
        }

        public async Task<T> WaitNextEvent(CancellationToken cancellationToken)
        {
            var triggered = false;
            T result = default(T);

            void Trigger(T value)
            {
                result = value;
                triggered = true;
            }

            var subscribe= SubscribeWithSkip(Trigger);
            while (triggered is false && cancellationToken.IsCancellationRequested is false)
            {
                await Task.Yield();
            }
            subscribe.Dispose();
            
            return result;
        }

        public void Dispose()
        {
            _reactiveProperty?.Dispose();
        }
    }
}