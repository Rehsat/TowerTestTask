using Game.Core.BlackHole;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Zenject;

namespace Game.Factories.Figures
{
    public class BlackHolePresenterFactory : IFactory<BlackHolePresenter>
    {
        private readonly IFactory<FigureData, FigureSpriteView> _spriteFiguresFactory;
        private readonly IFactory<IBlackHoleView> _blackHoleFactory;

        public BlackHolePresenterFactory(
            IFactory<FigureData, FigureSpriteView> spriteFiguresFactory
            , IFactory<IBlackHoleView> blackHoleFactory )
        {
            _spriteFiguresFactory = spriteFiguresFactory;
            _blackHoleFactory = blackHoleFactory;
        }
        public BlackHolePresenter Create()
        {
            var blackHoleView = _blackHoleFactory.Create();
            var blackHolePresenter = new BlackHolePresenter(blackHoleView, _spriteFiguresFactory);
            return blackHolePresenter;
        }
    }
}