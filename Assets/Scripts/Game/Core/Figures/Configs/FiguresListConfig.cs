using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace Game.Core.Figures.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/FiguresListConfig", fileName = "FiguresListConfig")]
    public class FiguresListConfig : ScriptableObject, IFiguresListConfig
    {
        [SerializeField] private bool _shuffle;
        [SerializeField] private List<FigureConfigContainer> _figureConfigs;
        public IReadOnlyCollection<FigureConfig> FigureConfigs
        {
            get
            {
                var list = new List<FigureConfig>();
                _figureConfigs.ForEach(container => list.Add(container.FigureConfig));
                if(_shuffle) list.Shuffle();
                return list;
            }
        }
    }
}