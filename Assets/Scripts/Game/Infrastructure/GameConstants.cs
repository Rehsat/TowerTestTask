using UnityEngine;

namespace Game.Infrastructure
{
    public static class GameConstants
    {
        public const int ReferenceScreenHeight = 1080;
        public const int ReferenceScreenWidth = 1920;
        public static Vector2 ReferenceScreenSize => new Vector2(ReferenceScreenWidth, ReferenceScreenHeight);
    }
}
