
namespace Game.Services.Save
{
    public interface ISavable
    {
        public SaveDataId Id { get; }
        public void Save(SaveData saveData);
        public void Load(SaveData saveData);
    }
}