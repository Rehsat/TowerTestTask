using Game.Core;

namespace Game.Infrastructure.CurrentLevelData
{
    public class CurrentLevelDataProvider : ICurrentLevelDataProvider
    {
        private LevelData _currentLevelData;
        public LevelData CurrentLevelData => _currentLevelData;
        public void SetCurrentLevelData(LevelData currentLevelData)
        {
            _currentLevelData = currentLevelData;
        }
    }
}