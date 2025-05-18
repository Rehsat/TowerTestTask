using Game.Services.FiguresCollections;

namespace Game.Core.Figures.Configs
{
    public interface IFigureListConfigById
    {
        public IFiguresListConfig GetFiguresConfig(FigureListContainerId id);
    }
}