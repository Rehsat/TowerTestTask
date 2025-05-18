using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/FigureConfigContainer", fileName = "FigureConfigContainer")]
    public class FigureConfigContainer : ScriptableObject, IFiguresConfigContainer
    {
        [SerializeField] private FigureConfig _figureConfig;
        public FigureConfig FigureConfig => _figureConfig;
    }
}