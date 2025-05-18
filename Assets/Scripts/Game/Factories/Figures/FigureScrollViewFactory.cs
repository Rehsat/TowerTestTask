using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureScrollViewFactory : BaseSimpleFactory<FiguresScrollView>
    {
        public FigureScrollViewFactory(IPrefabsContainer prefabsContainer) : base(prefabsContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.FiguresScroll;
        }
    }
}