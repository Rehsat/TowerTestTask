using System;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using UniRx;

namespace Infrastructure.StateMachine
{
    public class StateMachine<TStateType> where TStateType : IState
    {
        private ReactiveProperty<IState> _currentState;
        private ReactiveEvent<IState> _onStateChange;
        private CompositeDisposable _compositeDisposable;
        private Type _currentStateType;
    
        protected Dictionary<Type, TStateType> States;

        public ReactiveEvent<IState> OnStateChange => _onStateChange;

        public StateMachine(List<TStateType> states)
        {
            States = new Dictionary<Type, TStateType>();
            _compositeDisposable = new CompositeDisposable();
            _onStateChange = new ReactiveEvent<IState>();
            
            foreach (var state in states)
            {
                States.Add(state.GetType(), state);
            }

            _currentState = new ReactiveProperty<IState>(states[0]);
            _currentState.Subscribe(currentState => _onStateChange.Notify(currentState));
        }

        public void EnterState<TState>() where TState : TStateType
        {
            var stateType = typeof(TState);
            TryEnterState(stateType);
        }

        public void TryEnterState(Type stateType)
        {
            if(_currentStateType == stateType) return;
            if (States.ContainsKey(stateType))
            {
                _currentState.Value.Exit();
                _currentState.Value = States[stateType];
                _currentState.Value.Enter();
            }
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}