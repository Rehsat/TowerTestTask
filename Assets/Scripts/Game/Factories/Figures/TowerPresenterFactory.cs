using Game.Core.Figures.Data;
using Game.Core.Figures.Tower;
using Game.Core.Figures.View;
using Game.Services.FiguresCollections;
using Game.Services.OutOfScreenCheck;
using Zenject;

namespace Game.Factories.Figures
{
    public class TowerPresenterFactory : IFactory<TowerPresenter>
    {
        private readonly IFiguresListsProvider _figuresListsProvider;
        private readonly IFactory<FigureData, FigureSpriteView> _spriteFiguresFactor;
        private readonly IOutOfScreenCheckService _ofScreenCheckService;
        private readonly IFactory<ITowerView> _towerViewFactory;

        public TowerPresenterFactory(
            IFiguresListsProvider figuresListsProvider
            ,IFactory<FigureData, FigureSpriteView> spriteFiguresFactor
            ,IOutOfScreenCheckService ofScreenCheckService
            ,IFactory<ITowerView> towerViewFactory)
        {
            _figuresListsProvider = figuresListsProvider;
            _spriteFiguresFactor = spriteFiguresFactor;
            _ofScreenCheckService = ofScreenCheckService;
            _towerViewFactory = towerViewFactory;
        }
        public TowerPresenter Create()
        {
            var figuresList = _figuresListsProvider.GetListOfFigures(FigureListContainerId.Tower);
            var towerView = _towerViewFactory.Create();
            var towerPresenter = new TowerPresenter(
                figuresList
                ,towerView
                ,_ofScreenCheckService
                ,_spriteFiguresFactor);
            
            return towerPresenter;
        }
    }
}