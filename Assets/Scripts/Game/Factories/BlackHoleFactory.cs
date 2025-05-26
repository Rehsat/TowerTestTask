using Game.Core.BlackHole;
using Game.Infrastructure.AssetsManagement;
using Game.Services.RaycastService;

namespace Game.Factories
{
    public class BlackHoleFactory : BaseSimpleFactory<BlackHoleView>
    {
        private readonly IWorldCursorPositionProvider _worldCursorPositionProvider;

        public BlackHoleFactory(IPrefabsProvider prefabsProvider
            , IPrefabsTransformContainer prefabsTransformContainer
            , IWorldCursorPositionProvider worldCursorPositionProvider)
            : base(prefabsProvider, prefabsTransformContainer)
        {
            _worldCursorPositionProvider = worldCursorPositionProvider;
        }

        protected override Prefab GetPrefabType()
        {
            return Prefab.Blackhole;
        }

        protected override void DoAdditionalConstruct(BlackHoleView prefab)
        {
            base.DoAdditionalConstruct(prefab);
            prefab.Construct(_worldCursorPositionProvider);
        }
    }
}