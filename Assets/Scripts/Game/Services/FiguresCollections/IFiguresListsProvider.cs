using Game.Core.Figures.Data;
using UnityEngine;

namespace Game.Services.FiguresCollections
{
    public interface IFiguresListsProvider
    {
        public IListOfFiguresData GetListOfFigures(FigureListContainerId id);
    }
}
