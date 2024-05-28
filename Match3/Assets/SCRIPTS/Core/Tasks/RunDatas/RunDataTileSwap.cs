namespace Match3
{
    public struct RunDataTileSwap
    {
        public ITile Tile;
        public ITaskHandler<((int, int), (int, int))> TaskHandler;
        public (int x, int y) MoveTo;
        public (int x, int y) GridSize;
        public float Duration;

        public RunDataTileSwap(ITile tile, ITaskHandler<((int, int), (int, int))> taskHandler,
            (int, int) moveTo, (int x, int y) gridSize, float duration)
        {
            Tile = tile;
            TaskHandler = taskHandler;
            MoveTo = moveTo;
            GridSize = gridSize;
            Duration = duration;
        }
    }
}
