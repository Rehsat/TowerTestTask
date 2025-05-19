using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class DropCollider : MonoBehaviour, IDropContainer
    {
        private readonly ReactiveEvent<IDraggable> _onObjectDropped = new ReactiveEvent<IDraggable>();
        public IReadOnlyReactiveEvent<IDraggable> OnObjectDropped => _onObjectDropped;

        public void OnDrop(IDraggable droppedObject)
        {
            Debug.LogError(droppedObject);
            _onObjectDropped.Notify(droppedObject);
        }
    }
}
