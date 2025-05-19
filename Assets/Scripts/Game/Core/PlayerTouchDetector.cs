using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerTouchDetector : MonoBehaviour, ITouchDetector
    {
        private ReactiveTrigger _onTouch;

        public ReactiveTrigger OnTouch => _onTouch;

        public void Construct()
        {
            _onTouch = new ReactiveTrigger();
        }
        public void Interact()
        {
            _onTouch.Notify();
        }
    }
}