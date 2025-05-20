namespace Game.Services.Save
{
    public interface IDataSerializer
    {
        public string Serialize<TSerializeData>(TSerializeData data);
        public TSerializeData Deserialize<TSerializeData>(string json);
    }
}