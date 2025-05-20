using System;
using Game.Core.Figures.Data;
using Game.Services.FiguresCollections;
using RotaryHeart.Lib.SerializableDictionaryPro;

namespace Game.Services.Save
{
    [Serializable]
    public class SaveData
    {
        public SerializableDictionary<FigureListContainerId, SerializableListOfFigureData> DictionaryOfFiguresDatas;
    }
}