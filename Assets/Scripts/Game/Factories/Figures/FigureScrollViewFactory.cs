using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureScrollViewFactory : BaseSimpleFactory<FiguresScrollView>
    {
        public FigureScrollViewFactory(
            IPrefabsProvider prefabsProvider,
            IPrefabsTransformContainer prefabsTransformContainer) : base(prefabsProvider, prefabsTransformContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.FiguresScroll;
        }
    }
}