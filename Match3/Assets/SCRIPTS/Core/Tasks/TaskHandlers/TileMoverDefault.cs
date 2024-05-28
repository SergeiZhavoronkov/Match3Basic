using System.Collections.Generic;

namespace Match3
{
    public sealed class TileMoverDefault : 
        ITaskHandler<List<((int x, int y) from, (int x, int y) to)>>
    {
        private readonly TileProvider _tileProvider;
        private readonly int _xSize;
        private readonly int _ySize;
      
        private List<ITask<RunDataTileMove>> _tasks;
        
        private int _taskCount;
        private int _taskCompleted;

        public TileMoverDefault(TileProvider tileProvider, int xSize, int ySize) 
        {
            _tileProvider = tileProvider;
            _xSize = xSize;
            _ySize = ySize;

            _tasks = new List<ITask<RunDataTileMove>>();

            for (int i = 0; i < 10; i++)
            {
                _tasks.Add(new TaskMoveTile());
            }
        }

        public void Run(List<((int x, int y) from, (int x, int y) to)> moveData)
        {
            _taskCount = moveData.Count;
            _taskCompleted = 0;

            for (int i = 0; i < _taskCount; i++) 
            {
                var task = GetFreeTask();
                var runData = new RunDataTileMove(
                    _tileProvider.Get(moveData[i].from),
                    this, moveData[i].to, (_xSize, _ySize), 1f);
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

        private ITask<RunDataTileMove> GetFreeTask()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].IsCompleted()) return _tasks[i];
            }

            _tasks.Add(new TaskMoveTile());
            return _tasks[^1];
        }
    }
}
