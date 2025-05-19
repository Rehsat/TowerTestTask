using System;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Services.Input
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputActions _inputActions;
        private readonly ReactiveEvent<ActionState> _onDragActionStateChanged;
        private readonly ReactiveTrigger _onInputUpdate;
        private readonly CompositeDisposable _compositeDisposable;

        public IReadOnlyReactiveEvent<ActionState> OnDragActionStateChanged => _onDragActionStateChanged;
        public IReadOnlyReactiveTrigger OnInputUpdate => _onInputUpdate;
        public Vector2 PointerPosition => Mouse.current.position.ReadValue();
        public Vector2 PointerDelta => Mouse.current.delta.ReadValue();

        public InputService()
        {
            _onDragActionStateChanged = new ReactiveEvent<ActionState>();
            _onInputUpdate = new ReactiveTrigger();
            _compositeDisposable = new CompositeDisposable();
            
            _inputActions = new InputActions();
            _inputActions.Enable();
            
            _inputActions.Touch.Touch.performed += ctx => _onDragActionStateChanged.Notify(ActionState.Started);
            _inputActions.Touch.Touch.canceled += ctx => _onDragActionStateChanged.Notify(ActionState.Completed);

            Observable
                .EveryUpdate()
                .Subscribe(f => _onInputUpdate.Notify())
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _inputActions.Disable();
        }
    }
}
