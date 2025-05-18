using System;
using System.Collections.Generic;
using Game.Core.Figures.Configs;
using Game.Core.Figures.Data;
using UnityEngine;
using Zenject;

namespace Game.Services.FiguresCollections
{
    public class FiguresListsContainerService : IFiguresListsContainerService
    {
        private readonly IFigureListConfigById _listConfigById;
        private readonly IFactory<FigureConfig, FigureData> _figureDataFactory;
        private Dictionary<FigureListContainerId, IListOfFiguresData> _listOfFiguresDatas;
        
        public FiguresListsContainerService(
            IFigureListConfigById listConfigById, 
            IFactory<FigureConfig, FigureData> figureDataFactory)
        {
            _listConfigById = listConfigById;
            _figureDataFactory = figureDataFactory;
            
            _listOfFiguresDatas = new Dictionary<FigureListContainerId, IListOfFiguresData>();
            foreach (FigureListContainerId containerId in Enum.GetValues(typeof(FigureListContainerId)))
                InitializeListById(containerId);
        }

        //сделал конфиг и для башни для пункта про "обноввляемость". Можно будет, наапример, добавить стартовое положение башни
        private void InitializeListById(FigureListContainerId containerId)
        {
            if (_listOfFiguresDatas.ContainsKey(containerId))
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
            
            _listOfFiguresDatas.Add(containerId, listOfFiguresData);
        }
        
        public IListOfFiguresData GetListOfFigures(FigureListContainerId id)
        {
            return _listOfFiguresDatas[id];
        }
    }
}