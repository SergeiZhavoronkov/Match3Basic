using DG.Tweening;
using UnityEngine;

namespace Match3
{
    public class TaskSwapTile : ITask<RunDataTileSwap>
    {
        private ITile _tile;
        private Transform _tileTransform;
        private ITaskHandler<((int, int), (int, int))> _taskHandler;
        private int _id;
        private (int x, int y) _gridSize;
        private (int, int) _intMoveTo;
        private Vector3 _moveTo;

        private bool _isCompleted;

        public void Run(RunDataTileSwap runData)
        {
            _tile = runData.Tile;
            _tileTransform = _tile.GetTransform();
            _taskHandler = runData.TaskHandler;
            _id = _tileTransform.GetHashCode();
            _gridSize = runData.GridSize;
            _intMoveTo = runData.MoveTo;
            _moveTo = _intMoveTo.ConvertFromIntToVector(_gridSize.x, _gridSize.y);

            _isCompleted = false;

            _tileTransform.DOMove(_moveTo, runData.Duration).SetId(_id).
                OnComplete(() => 
                {
                    _taskHandler.InformTaskCompleted();                   
                    _tile.SetPosition(_intMoveTo, _gridSize);
                    _isCompleted = true;
                    DOTween.Kill(_id);
                });
        }

        public bool IsCompleted()
        {
            return _isCompleted;
        }
    }
}
