using System;

namespace Infrastructure
{
    public interface ISceneLoader
    {
        public void LoadScene(string sceneId, Action onSceneLoaded = null);
    }
}