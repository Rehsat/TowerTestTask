using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class CoroutineStarter : MonoBehaviour, ICoroutineStarter
    {
        public Coroutine CustomStartCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
    }
}