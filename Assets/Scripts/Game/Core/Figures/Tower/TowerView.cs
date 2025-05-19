using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.UI;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public class TowerView : MonoBehaviour, ITowerView
    {
        [SerializeField] private DropCollider _dropContainer;
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject => _dropContainer.OnObjectDropped;
        
        public void SetLastViewTransform(Transform lastViewTransform)
        {
            var lastViewPosition = lastViewTransform.position;
            var lastViewScale = lastViewTransform.localScale;
            
            _dropContainer.transform.localScale = lastViewScale;
            _dropContainer.transform.position = 
                new Vector2(
                    lastViewPosition.x,
                 lastViewPosition.y + lastViewScale.y);
        }

        public void PlaceFirstViewTransform(Transform transform)
        {
            transform.parent = this.transform;
            transform.localPosition = Vector2.zero;
        }
    }
}
