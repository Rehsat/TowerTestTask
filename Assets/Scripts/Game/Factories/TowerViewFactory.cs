using Game.Core.Figures.Tower;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories
{
    public class TowerViewFactory : BaseSimpleFactory<TowerView>
    {
        public TowerViewFactory(IPrefabsProvider prefabsProvider,
            IPrefabsTransformContainer prefabsTransformContainer) : base(prefabsProvider, prefabsTransformContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.TowerView;
        }
    }
}