﻿using EasyFramework.ReactiveEvents;

namespace Game.Core.Figures
{
    public interface IDragFigureDataCreator
    {
        public IReadOnlyReactiveEvent<DragFigureData> OnNewDragFigureData { get; }
    }
}