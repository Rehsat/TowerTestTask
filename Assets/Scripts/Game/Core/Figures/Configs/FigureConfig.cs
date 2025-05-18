using System;
using Game.Core.Figures.Data;
using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [Serializable]
    public struct FigureConfig
    {
        [SerializeField] private Figure _figureType;
        [SerializeField] private FigureColor _figureColor;
        public Figure FigureType => _figureType;
        public FigureColor FigureColor => _figureColor;
    }
}
