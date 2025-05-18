using Game.Infrastructure.AssetsManagement;

namespace Game.Infrastructure.Configs
{
    public interface IGameConfig
    {
        public IPrefabsContainer PrefabsContainer { get; }
    }
}