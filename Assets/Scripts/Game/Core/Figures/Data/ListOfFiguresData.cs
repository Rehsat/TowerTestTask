using UniRx;

namespace Game.Core.Figures.Data
{
    public class ListOfFiguresData : IListOfFiguresData
    {
        private ReactiveCollection<FigureData> _figureDatas;
        public IReadOnlyReactiveCollection<FigureData> FigureDatas => _figureDatas;

        public ListOfFiguresData()
        {
            _figureDatas = new ReactiveCollection<FigureData>();
        }
        
        public void AddData(FigureData figureData)
        {
            _figureDatas.Add(figureData);
        }

        public void RemoveData(FigureData figureData)
        {
            _figureDatas.Remove(figureData);
        }
    }
}