using Game.Core.Figures.Data;
using UnityEngine;

namespace Game.Core.Figures
{
    public interface IFigureSpriteByColorContainer
    {
        public Sprite GetSprite(FigureColor figureColor);
    }
}
