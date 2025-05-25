using UnityEngine;

namespace Game.Core.Screen
{
    public abstract class BaseScreenSizeChangeable : MonoBehaviour
    {
        public abstract void ChangeByScreenSize(ScreenSizeContext sizeContext);
    }
}