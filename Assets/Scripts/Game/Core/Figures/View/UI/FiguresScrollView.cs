using System;
using System.Collections.Generic;
using Game.Core.Figures.UI;
using Game.Core.UI;
using UniRx;
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
            listOfObjects.ForEach(ui =>
            {
                var startLocalScale = ui.transform.localScale;
                ui.transform.SetParent(_figuresRoot.transform, true);
                ui.transform.localScale = startLocalScale;
            });
        }

        public void SetInteractEnableState(bool isEnabled)
        {
            _scrollRect.enabled = isEnabled;
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
        }
    }
}
