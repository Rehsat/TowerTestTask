using Game.Core.Figures.Configs;
using Game.Infrastructure.AssetsManagement;

namespace Game.Infrastructure.Configs
{
    public interface IGameConfig
    {
        public IPrefabsContainer PrefabsContainer { get; }
        public IFigureSpriteByColorContainer FigureSpriteByColorContainer { get; }
        public IFigureListConfigById StartFigureListConfigById { get; }
    }
}