using System;
using Game.Core.Figures.Tower;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.BlackHole
{
    public interface IBlackHoleView : IDropHandler
    {
        public void DoSuckAnimation(Transform transformToAnimate, Action onComplete);
    }
}