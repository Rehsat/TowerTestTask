using EasyFramework.ReactiveTriggers;

namespace Game.Services.Save
{
    public interface ISaveRequier
    {
        public IReadOnlyReactiveTrigger OnSaveRequired { get; }
    }
}