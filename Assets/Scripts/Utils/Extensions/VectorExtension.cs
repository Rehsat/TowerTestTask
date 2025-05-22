using UnityEngine;

namespace Utils.Extensions
{
    public static class VectorExtension
    {
        public static float GetAspectRatio(this Vector2 vector2)
        {
            return vector2.x.GetRatio(vector2.y);
        }
    }
}