using System.Collections.Generic;

namespace Game.Core.Figures.Configs
{
    public interface IFiguresListConfig
    {
        public IReadOnlyCollection<FigureConfig> FigureConfigs { get; }
    }
}