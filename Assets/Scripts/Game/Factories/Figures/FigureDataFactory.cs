using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Zenject;

namespace Game.Factories.Figures
{
    public class FigureDataFactory : IFactory<FigureConfig, FigureData>
    {
        private readonly IFigureSpriteByColorContainer _spriteByColorContainer;

        public FigureDataFactory(IFigureSpriteByColorContainer spriteByColorContainer)
        {
            _spriteByColorContainer = spriteByColorContainer;
        }
        public FigureData Create(FigureConfig config)
        {
            var sprite = _spriteByColorContainer.GetSprite(config.FigureColor);
            var data = new FigureData(config, sprite);
            return data;
        }
    }
}
