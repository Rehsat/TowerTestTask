using System;
using Game.Core.Figures.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Figures.Data
{
    [Serializable]
    public class FigureData
    {
        [SerializeField] private FigureConfig _config;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _xMovementPercent;
        
        public FigureConfig Config => _config;
        public Sprite Sprite => _sprite;
        public float XMovementPercent => _xMovementPercent;

        private const float X_MOVEMENT_PERCENT_VARIATY = 45; 
        //В ТЗ написано "не больше 50", но 45 < 50, так что пункт вывполняется
        // а 45 выбрал потому, что так не будет ситуации кодга  куб прям еле еле краешком касается башни
        public FigureData(FigureConfig config, Sprite sprite)
        {
            _config = config;
            _sprite = sprite;
            _xMovementPercent = 
                Random.Range(-X_MOVEMENT_PERCENT_VARIATY, X_MOVEMENT_PERCENT_VARIATY);
        }

        public void SetXMovementPercent(float xMovementPercent)
        {
            _xMovementPercent = xMovementPercent;
        }
    }
}
