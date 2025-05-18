using System;
using Game.Core.Figures.Data;
using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [Serializable]
    public struct FigureConfig
    {
        [field: SerializeField] public Figure FigureType { get; }
        [field: SerializeField] public FigureColor FigureColor { get; }
    }
}
