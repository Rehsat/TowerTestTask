using System.Collections;
using Infrastructure;
using UnityEngine;

namespace Game.Infrastructure
{
    public class CoroutineStarter : MonoBehaviour, ICoroutineStarter
    {
        public Coroutine CustomStartCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
    }
}