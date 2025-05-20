using UnityEngine;

namespace Game.Services.Save
{
    public class JsonDataSerializer : IDataSerializer
    {
        public string Serialize<TSerializeData>(TSerializeData data)
            => JsonUtility.ToJson(data);

        public TSerializeData Deserialize<TSerializeData>(string json) 
            => JsonUtility.FromJson<TSerializeData>(json);
    }
}