using System;

namespace Game.Infrastructure
{
    public interface ISceneLoader
    {
        public void LoadScene(string sceneId, Action onSceneLoaded = null);
    }
}