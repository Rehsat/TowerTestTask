using EasyFramework.ReactiveEvents;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Collider))]
    public class DropContainerView : MonoBehaviour, IDropContainer
    {
        private readonly ReactiveEvent<IDraggable> _onObjectDropped = new ReactiveEvent<IDraggable>();
        public IReadOnlyReactiveEvent<IDraggable> OnObjectDropped => _onObjectDropped;

        public void OnDrop(IDraggable droppedObject)
        {
            _onObjectDropped.Notify(droppedObject);
        }
    }
}
