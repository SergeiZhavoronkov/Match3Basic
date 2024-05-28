using System.Collections.Generic;

namespace Match3
{
    public struct RunDataTileDebetMove 
    {
        public ITile Tile;
        public ITaskHandler<(List<(int, int)>, int[,] grid)> TaskHandler;
        public (int x, int y) MoveTo;
        public (int x, int y) GridSize;
        public float Duration;
        public int TileID;

        public RunDataTileDebetMove(ITile tile, ITaskHandler<(List<(int, int)>, int[,] grid)> taskHandler,
            (int, int) moveTo, (int, int) gridSize, float duration, int tileID)
        {
            Tile = tile;
            TaskHandler = taskHandler;
            MoveTo = moveTo;
            GridSize = gridSize;
            Duration = duration;
            TileID = tileID;
        }
    }
}
