using UnityEngine;

namespace Game.Core.Figures.Configs
{
    public class FigureConfigContainer : ScriptableObject
    {
        [field: SerializeField] public FigureConfig FigureConfig{ get; }
    }
}