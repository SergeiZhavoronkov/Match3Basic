namespace Match3
{
    public struct RunDataTileRemove
    {
        public ITile Tile;
        public ITaskHandler<(int, int)[]> TaskHandler;
        public float Duration;

        public RunDataTileRemove(ITile tile, ITaskHandler<(int, int)[]> taskHandler,
            float duration)
        {
            Tile = tile; 
            TaskHandler = taskHandler;
            Duration = duration;
        }
    }
}
