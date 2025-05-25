using System;
using System.Collections.Generic;
using Game.Core.Figures.Data;
using Game.Services.FiguresCollections;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UniRx;
using UnityEngine;

namespace Game.Services.Save
{
    public class SaveService : ISaveService
    {
        private readonly IDataSerializer _dataSerializer;

        private bool _saveWillBeInNextFrame;
        private Dictionary<SaveDataId, ISavable> _saveables;

        private const string SAVE_KEY = "SAVE_KEY";
        public SaveService(List<ISavable> savables, 
            IDataSerializer dataSerializer)
        {
            _dataSerializer = dataSerializer;
            _saveables = new Dictionary<SaveDataId, ISavable>();
            savables.ForEach(InitSavablle);

            //При необходимости можно будет добавить более сложную логику сейвов
            Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe((l => StartSave()));
        }

        private void InitSavablle(ISavable savable)
        {
            _saveables.Add(savable.Id, savable);
            if (savable is ISaveRequier saveRequier)
                saveRequier.OnSaveRequired.SubscribeWithSkip(StartSave);
        }
        public void StartSave()
        {
            if(_saveWillBeInNextFrame) return;
            _saveWillBeInNextFrame = true;

            Observable.TimerFrame(1).Subscribe(f =>
            {
                _saveWillBeInNextFrame = false;
                Save();
            });
        }

        public void Load()
        {
            var savedDataSerialized = PlayerPrefs.GetString(SAVE_KEY);
            var savedData = _dataSerializer.Deserialize<SaveData>(savedDataSerialized);
            if(savedData == null) return;
            
            foreach (var saveablesValue in _saveables.Values)
            {
                saveablesValue.Load(savedData);
            }
        }
        
        private void Save()
        {
            var saveData = new SaveData();
            foreach (var keyValuePair in _saveables)
                keyValuePair.Value.Save(saveData);
            
            var saveDataSerialized = _dataSerializer.Serialize(saveData);
            PlayerPrefs.SetString(SAVE_KEY, saveDataSerialized);
        }
        
    }
}