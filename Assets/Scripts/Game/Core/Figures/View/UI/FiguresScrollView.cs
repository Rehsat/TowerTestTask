using System.Collections.Generic;
using Game.Core.Figures.UI;
using Game.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Game.Core.Figures.View.UI
{
    public class FiguresScrollView : MonoBehaviour, IScrollView<FigureUI>
    {
        [SerializeField] private LayoutGroup _figuresRoot;
        public void SetObjectsToScroll(List<FigureUI> listOfObjects)
        {
            listOfObjects.ForEach(ui => ui.transform.parent = _figuresRoot.transform);
            _figuresRoot.UpdateGroup();
        }
    }
}
