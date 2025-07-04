using System;
using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.UI;
using Game.Factories;
using Game.Services.DragAndDrop;
using UnityEngine;

namespace Game.Core.Figures.Tower
{
    public class TowerView : MonoBehaviour, ITowerView, IConstructable
    {
        [SerializeField] private DropCollider _dropContainer;
        
        private Vector2 _startDropPosition;
        private Vector2 _startDropScale;
        
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject => _dropContainer.OnObjectDropped;
        public Transform DropContainerTransform => _dropContainer.transform;
        
        public void Construct()
        {
            _startDropPosition = _dropContainer.transform.localPosition;
            _startDropScale = _dropContainer.transform.localScale;
        }
        
        public void SetLastViewTransform(Transform lastViewTransform)
        {
            if (lastViewTransform == null)
            {
                _dropContainer.transform.parent = transform;
                ResetDropContainer();
                return;
            }
            
            var lastViewPosition = lastViewTransform.position;
            var lastViewScale = lastViewTransform.localScale;

            var additionalDropContainerHeight = -1;
            _dropContainer.transform.localScale = lastViewScale;
            _dropContainer.transform.position = 
                new Vector2(
                    lastViewPosition.x,
                 lastViewPosition.y + lastViewScale.y - additionalDropContainerHeight);
            _dropContainer.transform.SetParent(lastViewTransform, false);
        }

        public void PlaceFirstViewTransform(Transform transform)
        {
            transform.parent = this.transform;
            transform.localPosition = Vector2.zero;
        }

        private void ResetDropContainer()
        {
            _dropContainer.transform.localPosition = _startDropPosition;
            _dropContainer.transform.localScale = _startDropScale;
        }
    }
}
