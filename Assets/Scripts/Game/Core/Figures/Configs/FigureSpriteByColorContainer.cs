using Game.Core.Figures.Data;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Game.Core.Figures.Configs
{
    [CreateAssetMenu(menuName = "GameConfigs/FigureSpriteByColorContainer", fileName = "FigureSpriteByColorContainer")]
    public class FigureSpriteByColorContainer : ScriptableObject, IFigureSpriteByColorContainer
    {
        [SerializeField] private SerializableDictionary<FigureColor, Sprite> _figureSprites;
        public Sprite GetSprite(FigureColor figureColor)
        {
            return _figureSprites[figureColor];
        }
    }
}