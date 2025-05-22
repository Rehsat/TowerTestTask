using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Infrastructure.ScreenScale
{
    public interface IScreenScaleCalculator
    {
        public float CameraSizeChange { get; }
        public Vector2 ScreenSize { get; }
        public Vector2 SpritesTransformPositionChange { get; }
    }
}
