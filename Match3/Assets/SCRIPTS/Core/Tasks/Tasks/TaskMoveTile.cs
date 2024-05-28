using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public sealed class TaskMoveTile : ITask<RunDataTileMove>
    {
        private ITile _tile;
        private Transform _tileTransform;
        private ITaskHandler<List<((int x, int y) from, (int x, int y) to)>> _taskHandler;
        private int _tweenId;
        private (int x, int y) _gridSize;
        private (int, int) _intMoveTo;
        private Vector3 _moveTo;

        private bool _isCompleted;

        public TaskMoveTile()
        {
            _isCompleted = true;
        }

        public void Run(RunDataTileMove runData)
        {
            _tile = runData.Tile;
            _tileTransform = runData.Tile.GetTransform();
            _taskHandler = runData.TaskHandler;
            _tweenId = _tileTransform.GetHashCode();
            _gridSize = runData.GridSize;
            _intMoveTo = runData.MoveTo;
            _moveTo = _intMoveTo.ConvertFromIntToVector(_gridSize.x, _gridSize.y);
            
            _isCompleted = false;

            _tileTransform.DOMove(_moveTo, runData.Duration).SetId(_tweenId).
                OnComplete(() => 
                {
                    _taskHandler.InformTaskCompleted();
                    _tile.SetPosition(_intMoveTo, _gridSize);
                    _isCompleted = true;
                    DOTween.Kill(_tweenId);
                });
        }

        public bool IsCompleted()
        {
            return _isCompleted;
        }
    }
}
