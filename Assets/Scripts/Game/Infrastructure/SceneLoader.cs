using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineStarter _coroutineStarter;

        public SceneLoader(ICoroutineStarter coroutineStarter)
        {
            _coroutineStarter = coroutineStarter;
        }
        
        public void LoadScene(string sceneId, Action onSceneLoaded = null)
        {
            _coroutineStarter.CustomStartCoroutine(LoadSceneAsync(sceneId, onSceneLoaded));
        }
// Сделал для обновляемости, можно сделать экран загрузки
        public IEnumerator LoadSceneAsync(string sceneId, Action onSceneLoaded = null) 
        {
            var waitForLoad = SceneManager.LoadSceneAsync(sceneId);
            while (waitForLoad.isDone == false)
                yield return null;
            
            onSceneLoaded?.Invoke();
        }
    }
}