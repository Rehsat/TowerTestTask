using Game.Core.Figures.UI;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureUiViewFactory : BaseFigureViewFactory<FigureUI>
    {
        protected override FigureUI ViewPrefab { get; }

        public FigureUiViewFactory(IPrefabsContainer prefabsContainer)
        {
            ViewPrefab = prefabsContainer.GetPrefabsComponent<FigureUI>(Prefab.FigureUI);
        }
    }
}