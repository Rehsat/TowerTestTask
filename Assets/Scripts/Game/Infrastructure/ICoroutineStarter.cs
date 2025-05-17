using System.Collections;
using UnityEngine;

namespace Game.Infrastructure
{
    public interface ICoroutineStarter
    {
        public Coroutine CustomStartCoroutine(IEnumerator coroutine); // название чтоб не путать с юнитевским StartCoroutine
    }
}