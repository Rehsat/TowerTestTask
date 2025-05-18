using System;
using Game.Core.Figures.Configs;
using UnityEngine;

namespace Game.Core.Figures.Data
{
    [Serializable]
    public class FigureData
    {
        private FigureConfig _config;
        
        public FigureConfig Config => _config;
        public Sprite Sprite { get; }

        public FigureData(FigureConfig config, Sprite sprite)
        {
            _config = config;
            Sprite = sprite;
        }
    }
}
