using Game.Core;

namespace Game.Infrastructure.CurrentLevelData
{
    public interface ICurrentLevelDataProvider
    {
        public LevelData CurrentLevelData { get; }
        public void SetCurrentLevelData(LevelData currentLevelData);
    }
}
