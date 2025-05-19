using Game.Core.Figures.Tower;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories
{
    public class TowerViewFactory : BaseSimpleFactory<TowerView>
    {
        public TowerViewFactory(IPrefabsContainer prefabsContainer,
            IPrefabsTransformContainer prefabsTransformContainer) : base(prefabsContainer, prefabsTransformContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.TowerView;
        }
    }
}