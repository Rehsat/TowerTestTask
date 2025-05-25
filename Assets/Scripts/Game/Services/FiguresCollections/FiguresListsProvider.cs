using System;
using System.Collections.Generic;
using System.Linq;
using EasyFramework.ReactiveTriggers;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using Game.Services.Save;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace Game.Services.FiguresCollections
{
    public class FiguresListsProvider : IFiguresListsProvider, ISavable, ISaveRequier, IDisposable
    {
        private readonly IFigureListConfigById _listConfigById;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;
        
        private ReactiveTrigger _onSaveRequired;
        private CompositeDisposable _compositeDisposable;
        private Dictionary<FigureListContainerId, IListOfFiguresData> _dictionaryOfLists;
        
        public SaveDataId Id => SaveDataId.ListOfFiguresData;
        public IReadOnlyReactiveTrigger OnSaveRequired => _onSaveRequired;
        
        public FiguresListsProvider(
            IFigureListConfigById listConfigById, 
            IFactory<FigureConfig, FigureData> figureDataFactory)
        {
            _listConfigById = listConfigById;
            _figureDataFactory = figureDataFactory;
            _onSaveRequired = new ReactiveTrigger();
            _compositeDisposable = new CompositeDisposable();
            
            _dictionaryOfLists = new Dictionary<FigureListContainerId, IListOfFiguresData>();
            foreach (FigureListContainerId containerId in Enum.GetValues(typeof(FigureListContainerId)))
                InitializeListById(containerId);
            
            InitializeSave();
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

        private void InitializeSave()
        {
            foreach (var listOfFiguresData in _dictionaryOfLists.Values)
            {
                listOfFiguresData.FigureDatas
                    .ObserveCountChanged()
                    .Subscribe(f =>
                    _onSaveRequired.Notify())
                    .AddTo(_compositeDisposable);
            }
        }

        public IListOfFiguresData GetListOfFigures(FigureListContainerId id)
        {
            return _dictionaryOfLists[id];
        }
        
        //Пo SRP для этой истории ниже лучше выделить отдельный класс, но я решил оставить так, т.к. мне кажется в данном случае это будет оверинжениринг.
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
            var actualListOfFigures = ConvertSavedListToData(towerSavedFigures);
            
            var towerFigures = _dictionaryOfLists[towerId];
            towerFigures.SetData(actualListOfFigures);
        }

        private List<FigureData> ConvertSavedListToData(SerializableListOfFigureData savedList)
        {
            var listOfData = new List<FigureData>();
            savedList.FigureDatas.ForEach((savedData =>
            {
                var actualData = _figureDataFactory.Create(savedData.Config);
                actualData.SetXMovementPercent(savedData.XMovementPercent);
                listOfData.Add(actualData);
            }));
            
            return listOfData;
        }
        public void Dispose()
        {
            _onSaveRequired?.Dispose();
            _compositeDisposable?.Dispose();
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