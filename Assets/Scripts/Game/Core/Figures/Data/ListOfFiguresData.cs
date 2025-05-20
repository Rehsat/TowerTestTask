using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Core.Figures.Data
{
    [Serializable]
    public class ListOfFiguresData : IListOfFiguresData
    {
        [SerializeField] private SerializableFiguresReactiveCollection _figureDatas;
        public IReadOnlyReactiveCollection<FigureData> FigureDatas => _figureDatas.FigureDatas;

        public ListOfFiguresData()
        {
            _figureDatas = new SerializableFiguresReactiveCollection();
        }
        
        public void AddData(FigureData figureData)
        {
            _figureDatas.FigureDatas.Add(figureData);
        }

        public void RemoveData(FigureData figureData)
        {
            _figureDatas.FigureDatas.Remove(figureData);
        }

        public void SetData(IEnumerable<FigureData> collection)
        {
            _figureDatas.FigureDatas.Clear();
            foreach (var figureData in collection)
                _figureDatas.FigureDatas.Add(figureData);
        }
    }

    [Serializable]
    public class SerializableFiguresReactiveCollection
    {
        public ReactiveCollection<FigureData> FigureDatas;

        public SerializableFiguresReactiveCollection()
        {
            FigureDatas = new ReactiveCollection<FigureData>();
        }
    }
}