using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Game.Core.Figures.Data
{
    public interface IListOfFiguresData
    {
        public IReadOnlyReactiveCollection<FigureData> FigureDatas { get; }
        public void AddData(FigureData figureData);
        public void RemoveData(FigureData figureData);
        public void SetData(IEnumerable<FigureData> collection);
    }
}