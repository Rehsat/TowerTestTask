using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Core.Figures.View;
using Game.Core.Figures.View.UI;
using Game.Infrastructure.AssetsManagement;
using Game.Infrastructure.CurrentLevelData;
using Game.Services.FiguresCollections;
using Game.Services.Input;
using Zenject;

namespace Game.Factories.Figures
{
    public class FigureSpriteViewFactory : BaseFigureViewFactory<FigureSpriteView>
    {
        protected override FigureSpriteView ViewPrefab { get; }

        public FigureSpriteViewFactory(IPrefabsProvider prefabsProvider)
        {
            ViewPrefab = prefabsProvider.GetPrefabsComponent<FigureSpriteView>(Prefab.FigureSprite);
        }
    }

    public class FiguresScrollPresenterFactory : IFactory<FiguresScrollPresenter>
    {
        private readonly IFiguresListsProvider _figuresListsProvider;
        private readonly IFactory<FigureData, FigureUI> _uiFiguresFactory;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;
        private readonly IInputService _inputService;

        public FiguresScrollPresenterFactory(
            IFiguresListsProvider figuresListsProvider
            ,IFactory<FigureData, FigureUI> uiFiguresFactory
            ,ICurrentLevelDataProvider currentLevelDataProvider
            ,IFactory<FigureConfig, FigureData> figureDataFactory
            ,IInputService inputService)
        {
            _figuresListsProvider = figuresListsProvider;
            _uiFiguresFactory = uiFiguresFactory;
            _currentLevelDataProvider = currentLevelDataProvider;
            _figureDataFactory = figureDataFactory;
            _inputService = inputService;
        }

        public FiguresScrollPresenter Create()
        {
            var scroll = _currentLevelDataProvider.CurrentLevelData.GetPrefabsComponent<FiguresScrollView>(Prefab.FiguresScroll);
            var figuresList = _figuresListsProvider.GetListOfFigures(FigureListContainerId.Scroll);
            
            return new FiguresScrollPresenter(
                figuresList
                ,_uiFiguresFactory
                ,scroll 
                ,_inputService
                ,_figureDataFactory
            );
        }
    }
}