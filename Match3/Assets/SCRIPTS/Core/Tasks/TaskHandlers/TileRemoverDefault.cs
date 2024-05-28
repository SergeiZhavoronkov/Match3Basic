using System.Collections.Generic;

namespace Match3
{
    public sealed class TileRemoverDefault : ITaskHandler<(int, int)[]>
    {
        private readonly TileProvider _tileProvider;
        
        private List<ITask<RunDataTileRemove>> _tasks;
        
        private int _taskCount;
        private int _taskCompleted;

        public TileRemoverDefault(TileProvider tileProvider) 
        {
            _tileProvider = tileProvider;
            _tasks = new List<ITask<RunDataTileRemove>>();
            
            for (int i = 0; i < 10; i++) 
            {
                _tasks.Add(new TaskRemoveTile(_tileProvider));
            }
        }

        public void Run((int, int)[] matchesData)
        {
            _taskCount = matchesData.Length;
            _taskCompleted = 0;

            for (int i = 0; i < _taskCount; i++) 
            {
                var task = GetFreeTask();
                var runData = new RunDataTileRemove(
                    _tileProvider.Get(matchesData[i]), this, 1f);
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

        private ITask<RunDataTileRemove> GetFreeTask()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].IsCompleted()) return _tasks[i];
            }

            _tasks.Add(new TaskRemoveTile(_tileProvider));
            return _tasks[^1];
        }
    }
}
