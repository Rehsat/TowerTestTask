using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Services.Input
{
    public interface IInputService
    {
        public Vector2 PointerPosition { get; }
        public Vector2 PointerDelta { get; }
        public IReadOnlyReactiveEvent<ActionState> OnDragActionStateChanged { get; }
        public IReadOnlyReactiveTrigger OnInputUpdate { get; }
    }
}