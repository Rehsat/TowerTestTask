using System.Collections.Generic;
using EasyFramework.ReactiveEvents;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Core.UI;
using Game.Services.DragAndDrop;
using UnityEngine;
using Zenject;

namespace Game.Core.Figures.Tower
{
    public class TowerView : MonoBehaviour, ITowerView
    {
        [SerializeField] private DropContainerView _dropContainer;
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
    }

    public interface ITowerView
    {
        public IReadOnlyReactiveEvent<IDraggable> OnDroppedNewObject { get; }
        public void SetLastViewTransform(Transform transform);
    }

    public class TowerPresenter
    {
        private readonly IListOfFiguresData _listOfFiguresData;
        private readonly ITowerView _towerView;
        private readonly IFactory<FigureData, FigureSpriteView> _figureSpriteViewFactory;

        public TowerPresenter(
            IListOfFiguresData listOfFiguresData, 
            ITowerView towerView,
            IFactory<FigureData, FigureSpriteView> figureSpriteViewFactory)
        {
            _listOfFiguresData = listOfFiguresData;
            _towerView = towerView;
            _figureSpriteViewFactory = figureSpriteViewFactory;
        }
    }
}
