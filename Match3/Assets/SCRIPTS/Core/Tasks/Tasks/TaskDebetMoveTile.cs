using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public sealed class TaskDebetMoveTile : ITask<RunDataTileDebetMove>
    {
        private ITile _tile;
        private Transform _tileTransform;
        private ITaskHandler<(List<(int, int)>, int[,])> _taskHandler;
        private int _tweenId;
        private (int x, int y) _gridSize;
        private (int, int) _intMoveTo;
        private Vector3 _moveTo;

        private bool _isCompleted;

        public void Run(RunDataTileDebetMove runData)
        {
            _tile = runData.Tile;
            _tileTransform = runData.Tile.GetTransform();
            _taskHandler = runData.TaskHandler;
            _tweenId = _tileTransform.GetHashCode();
            _gridSize = runData.GridSize;
            _intMoveTo = runData.MoveTo;
            _moveTo = runData.MoveTo.ConvertFromIntToVector(_gridSize.x, _gridSize.y);

            _isCompleted = false;

            _tile.SetID(runData.TileID);
            _tileTransform.position = new Vector3(_moveTo.x, 14f, 0f);

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
