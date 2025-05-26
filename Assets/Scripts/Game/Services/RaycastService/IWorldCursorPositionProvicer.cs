using UnityEngine;

namespace Game.Services.RaycastService
{
    public interface IWorldCursorPositionProvider
    {
        public Vector3 WorldCursorPosition { get; }
    }
}