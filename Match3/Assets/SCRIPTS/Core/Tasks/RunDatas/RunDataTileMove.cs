using System.Collections.Generic;

namespace Match3
{
    public struct RunDataTileMove
    {
        public ITile Tile;
        public ITaskHandler<List<((int x, int y) from, (int x, int y) to)>> TaskHandler;
        public (int x, int y) MoveTo;
        public (int x, int y) GridSize;
        public float Duration;
        
        public RunDataTileMove(ITile tile, ITaskHandler<List<((int x, int y) from, (int x, int y) to)>> taskHandler,
            (int x, int y) moveTo, (int x, int y) gridSize, float duration)
        {
            Tile = tile;
            TaskHandler = taskHandler;
            MoveTo = moveTo;
            GridSize = gridSize;
            Duration = duration;
        }
    }
}
