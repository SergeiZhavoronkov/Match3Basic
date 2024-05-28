namespace Match3
{
    public sealed class TileSwapperDefault : ITaskHandler<((int x, int y) from, (int x, int y) to)>
    {
        private const int _taskCount = 2;

        private readonly TileProvider _tileProvider;
        private readonly int _xSize;
        private readonly int _ySize;

        private ITask<RunDataTileSwap> _taskMoveTile1;
        private ITask<RunDataTileSwap> _taskMoveTile2;

        private int _taskCompleted;

        public TileSwapperDefault(TileProvider tileProvider, int xSize, int ySize)
        {
            _tileProvider = tileProvider;
            _xSize = xSize;
            _ySize = ySize;

            _taskMoveTile1 = new TaskSwapTile();
            _taskMoveTile2 = new TaskSwapTile();
        }

        public void Run(((int x, int y) from, (int x, int y) to) swapData)
        {
            _taskCompleted = 0;
            
            var runData1 = new RunDataTileSwap(_tileProvider.Get(swapData.from),
                this, swapData.to, (_xSize, _ySize), 1f);

            var runData2 = new RunDataTileSwap(_tileProvider.Get(swapData.to),
               this, swapData.from, (_xSize, _ySize), 1f);

            _taskMoveTile1.Run(runData1);
            _taskMoveTile2.Run(runData2);
        }

        public void InformTaskCompleted()
        {
            _taskCompleted++;
        }

        public bool AllTasksCompleted()
        {
            return _taskCount.Equals(_taskCompleted);
        }
    }
}
