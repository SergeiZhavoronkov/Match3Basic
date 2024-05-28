using System.Collections.Generic;

namespace Match3
{
    public sealed class TileDebetMoverDefault 
        : ITaskHandler<(List<(int x, int y)> moveTo, int[,] grid)>
    {
        private readonly TileProvider _tileProvider;
        private readonly int _xSize;
        private readonly int _ySize;

        private List<ITask<RunDataTileDebetMove>> _tasks;

        private int _taskCount;
        private int _taskCompleted;

        public TileDebetMoverDefault(TileProvider tileProvider, int xSize, int ySize)
        {
            _tileProvider = tileProvider;
            _xSize = xSize;
            _ySize = ySize;

            _tasks = new List<ITask<RunDataTileDebetMove>>();

            for (int i = 0; i < 10; i++) 
            {
                _tasks.Add(new TaskDebetMoveTile());
            }
        }

        public void Run((List<(int x, int y)> moveTo, int[,] grid) debetMoveData)
        {
            _taskCount = debetMoveData.moveTo.Count;
            _taskCompleted = 0;

            for (int i = 0; i < _taskCount; i++)
            {
                var task = GetFreeTask();
                var tileID = debetMoveData.grid[debetMoveData.moveTo[i].x, debetMoveData.moveTo[i].y];
                var runData = new RunDataTileDebetMove(
                    _tileProvider.GetFreeTile(),
                    this, debetMoveData.moveTo[i], (_xSize, _ySize), 1f, tileID);
                task.Run(runData);
            }
        }

        public void InformTaskCompleted()
        {
            _taskCompleted++;
        }

        public bool AllTasksCompleted()
        {
            return _taskCount.Equals(_taskCompleted);
        }

        private ITask<RunDataTileDebetMove> GetFreeTask()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].IsCompleted()) return _tasks[i];
            }
            _tasks.Add(new TaskDebetMoveTile());
            return _tasks[^1];
        }
    }
}
