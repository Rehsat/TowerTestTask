using Game.Core.Figures.Data;
using Game.Core.Figures.UI;
using UnityEngine;
using Zenject;

namespace Game.Factories.Figures
{
    public abstract class BaseFigureViewFactory<TFigureViewType> : IFactory<FigureData, TFigureViewType> 
        where TFigureViewType : MonoBehaviour, IFigureView
    {
        protected abstract TFigureViewType ViewPrefab { get; }
        public TFigureViewType Create(FigureData figureData)
        {
            var view = Object.Instantiate(ViewPrefab);
            view.SetSprite(figureData.Sprite);
            return view;
        }
    }
}
