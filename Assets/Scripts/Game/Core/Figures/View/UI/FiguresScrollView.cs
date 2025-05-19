using System.Collections.Generic;
using Game.Core.Figures.UI;
using Game.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Game.Core.Figures.View.UI
{
    public class FiguresScrollView : MonoBehaviour, ICollectionView<FigureUI>, IInteractEnabable
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private LayoutGroup _figuresRoot;
        public void SetListOfObjects(List<FigureUI> listOfObjects)
        {
            listOfObjects.ForEach(ui => ui.transform.parent = _figuresRoot.transform);
            _figuresRoot.UpdateGroup();
        }

        public void SetInteractEnableState(bool isEnabled)
        {
            _scrollRect.enabled = isEnabled;
        }
    }
}
