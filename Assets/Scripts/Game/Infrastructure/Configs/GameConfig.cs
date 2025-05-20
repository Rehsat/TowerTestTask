using Game.Core.Figures.Configs;
using Game.Infrastructure.AssetsManagement;
using UnityEngine;

namespace Game.Infrastructure.Configs
{
    /*
    Думаю, что покрытием интерфейсами IGameConfig смог закрыть задачу 3 из требований:
    Нужно учесть что источником конфигурации игры могут стать разные источники данных 
    (в игре может быть 1 реализация - из ScriptableObject);
    */
    [CreateAssetMenu(menuName = "GameConfigs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [SerializeField] private PrefabsProviderConfig prefabsProviderConfig;
        [SerializeField] private FigureSpriteByColorContainer _figureSpriteByColor;
        [SerializeField] private FigureListConfigById figureListConfigById;
        public IPrefabsProvider PrefabsProvider => prefabsProviderConfig;
        public IFigureSpriteByColorContainer FigureSpriteByColorContainer => _figureSpriteByColor;
        public IFigureListConfigById StartFigureListConfigById => figureListConfigById;
    }
}
