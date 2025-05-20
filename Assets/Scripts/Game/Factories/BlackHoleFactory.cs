using Game.Core.BlackHole;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories
{
    public class BlackHoleFactory : BaseSimpleFactory<BlackHoleView>
    {
        public BlackHoleFactory(IPrefabsProvider prefabsProvider, IPrefabsTransformContainer prefabsTransformContainer)
            : base(prefabsProvider, prefabsTransformContainer)
        {
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.Blackhole;
        }
    }
}