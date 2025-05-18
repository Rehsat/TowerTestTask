using System;
using System.Collections.Generic;
using Game.Core.Figures.Data;

namespace Game.Services.FiguresCollections
{
    public class FiguresListsContainerService : IFiguresListsContainerService
    {
        private Dictionary<FigureListContainerId, IListOfFiguresData> _listOfFiguresDatas;
        
        public FiguresListsContainerService()
        {
            _listOfFiguresDatas = new Dictionary<FigureListContainerId, IListOfFiguresData>();
            foreach (FigureListContainerId containerId in Enum.GetValues(typeof(FigureListContainerId)))
            {
                IListOfFiguresData figuresData = new ListOfFiguresData();
                _listOfFiguresDatas.Add(containerId, figuresData);
            }
        }
        
        public IListOfFiguresData GetListOfFigures(FigureListContainerId id)
        {
            return _listOfFiguresDatas[id];
        }
    }
}