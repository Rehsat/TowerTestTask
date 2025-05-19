using Game.Core.Figures.View;
using Game.Infrastructure.AssetsManagement;

namespace Game.Factories.Figures
{
    public class FigureSpriteViewFactory : BaseFigureViewFactory<FigureSpriteView>
    {
        protected override FigureSpriteView ViewPrefab { get; }

        public FigureSpriteViewFactory(IPrefabsContainer prefabsContainer)
        {
            ViewPrefab = prefabsContainer.GetPrefabsComponent<FigureSpriteView>(Prefab.FigureSprite);
        }
    }
}