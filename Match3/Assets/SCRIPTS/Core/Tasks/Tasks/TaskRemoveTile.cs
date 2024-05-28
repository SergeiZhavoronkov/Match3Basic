using DG.Tweening;
using UnityEngine;

namespace Match3
{
    public sealed class TaskRemoveTile : ITask<RunDataTileRemove>
    {
        private const float _targetScale = 0f;
        private readonly TileProvider _tileProvider;

        private ITile _tile;
        private Transform _tileTransform;
        private ITaskHandler<(int, int)[]> _taskHandler;
        
        private int _tweenId;
        private bool _isCompleted;

        public TaskRemoveTile(TileProvider tileProvider)
        {
            _isCompleted = true;
            _tileProvider = tileProvider;
        }

        public void Run(RunDataTileRemove runData)
        {
            _tile = runData.Tile;
            _tileTransform = runData.Tile.GetTransform();
            _taskHandler = runData.TaskHandler;
            _tweenId = _tileTransform.GetHashCode();
            
            _isCompleted = false;

            _tileTransform.DOScale(_targetScale, runData.Duration).SetId(_tweenId).
                OnComplete(() => 
            {
                _taskHandler.InformTaskCompleted();
                _tileProvider.RemoveTileFromGrid(_tile);
                _tileTransform.localScale = Vector3.one;
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
