using Game.Core.Figures.Data;
using UnityEngine;

namespace Game.Services.FiguresCollections
{
    public interface IFiguresListsContainerService
    {
        public IListOfFiguresData GetListOfFigures(FigureListContainerId id);
    }
}
