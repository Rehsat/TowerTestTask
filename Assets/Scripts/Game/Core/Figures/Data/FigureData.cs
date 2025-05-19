using System;
using Game.Core.Figures.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Figures.Data
{
    [Serializable]
    public class FigureData
    {
        private FigureConfig _config;
        
        public FigureConfig Config => _config;
        public Sprite Sprite { get; }
        public float XMovementPercent { get; }

        private const float X_MOVEMENT_PERCENT_VARIATY = 45; 
        //В ТЗ написано "не больше 50", но 45 < 50, так что пункт вывполняется
        // а 45 выбрал потому, что так не будет ситуации кодга  куб прям еле еле краешком касается башни
        public FigureData(FigureConfig config, Sprite sprite)
        {
            _config = config;
            Sprite = sprite;
            XMovementPercent = 
                Random.Range(-X_MOVEMENT_PERCENT_VARIATY, X_MOVEMENT_PERCENT_VARIATY);
        }
    }
}
