using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureScrollViewFactory : BaseSimpleFactory<FiguresScrollView>
    {
        public FigureScrollViewFactory(
            IPrefabsContainer prefabsContainer,
            IPrefabsTransformContainer prefabsTransformContainer) : base(prefabsContainer, prefabsTransformContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.FiguresScroll;
        }
    }
}