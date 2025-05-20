using UnityEngine;

namespace Game.Services.OutOfScreenCheck
{
    public interface IOutOfScreenCheckService
    {
        public bool IsObjectOutOfScreen(Transform transformToCheck);
    }
}