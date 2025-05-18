using Game.Services.FiguresCollections;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/Figures/FigureListByIdConfig", fileName = "FigureListByIdConfig")]
    public class FigureListConfigById : ScriptableObject, IFigureListConfigById
    {
        [SerializeField] private SerializableDictionary<FigureListContainerId, FiguresListConfig> _configs;
        public IFiguresListConfig GetFiguresConfig(FigureListContainerId id)
        {
            return _configs[id];
        }
    }
}