using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Services.Save;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;
using Zenject;

namespace Game.Services.FiguresCollections
{
    public class FiguresListsContainerService : IFiguresListsContainerService, ISavable
    {
        private readonly IFigureListConfigById _listConfigById;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;
        private Dictionary<FigureListContainerId, IListOfFiguresData> _dictionaryOfLists;
        public SaveDataId Id => SaveDataId.ListOfFiguresData;
        public FiguresListsContainerService(
            IFigureListConfigById listConfigById, 
            IFactory<FigureConfig, FigureData> figureDataFactory)
        {
            _listConfigById = listConfigById;
            _figureDataFactory = figureDataFactory;
            
            _dictionaryOfLists = new Dictionary<FigureListContainerId, IListOfFiguresData>();
            foreach (FigureListContainerId containerId in Enum.GetValues(typeof(FigureListContainerId)))
                InitializeListById(containerId);
        }

        //сделал конфиг и для башни для пункта про "обноввляемость". Можно будет, наапример, добавить стартовое положение башни
        private void InitializeListById(FigureListContainerId containerId)
        {
            if (_dictionaryOfLists.ContainsKey(containerId))
            {
                Debug.LogError($"Already contains list with {containerId}");
                return;
            }
            
            var listOfFiguresData = new ListOfFiguresData();
            var config = _listConfigById.GetFiguresConfig(containerId);
            foreach (var figureConfig in config.FigureConfigs)
            {
                var figureData = _figureDataFactory.Create(figureConfig);
                listOfFiguresData.AddData(figureData);
            }
            
            _dictionaryOfLists.Add(containerId, listOfFiguresData);
        }
        
        public IListOfFiguresData GetListOfFigures(FigureListContainerId id)
        {
            return _dictionaryOfLists[id];
        }
        
        //Вообще для этой истории ниже лучше выделить отдельный класс, но тут не особо много логики, так что решил немного нарушить SRP в пользу простоты
        public void Save(SaveData saveData)
        {
            saveData.DictionaryOfFiguresDatas =
                new SerializableDictionary<FigureListContainerId, SerializableListOfFigureData>();
            
            foreach (var listOfFiguresData in _dictionaryOfLists)
            {
                var serializableList = new SerializableListOfFigureData();
                serializableList.FigureDatas = listOfFiguresData.Value.FigureDatas.ToList();
                saveData.DictionaryOfFiguresDatas.Add(listOfFiguresData.Key, serializableList);
            }
        }

        //Пока сохранение только для башни, при жеелании можно добавить для скролла
        public void Load(SaveData saveData)
        {
            var dictionaryOfDatas = saveData.DictionaryOfFiguresDatas;
            if(dictionaryOfDatas == null) return;

            var towerId = FigureListContainerId.Tower;
            if(dictionaryOfDatas.ContainsKey(towerId) == false) return;
            var towerSavedFigures = dictionaryOfDatas[towerId];
            var towerFigures = _dictionaryOfLists[towerId];
            towerFigures.SetData(towerSavedFigures.FigureDatas);
        }
    }

    [Serializable]
    public class SerializableListOfFigureData
    {
        public List<FigureData> FigureDatas;

        public SerializableListOfFigureData()
        {
            FigureDatas = new List<FigureData>();
        }
    }

}