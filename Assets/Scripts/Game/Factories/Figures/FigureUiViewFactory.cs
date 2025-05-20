using Game.Core.Figures.UI;
using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureUiViewFactory : BaseFigureViewFactory<FigureUI>
    {
        protected override FigureUI ViewPrefab { get; }

        public FigureUiViewFactory(IPrefabsProvider prefabsProvider)
        {
            ViewPrefab = prefabsProvider.GetPrefabsComponent<FigureUI>(Prefab.FigureUI);
        }
    }
}