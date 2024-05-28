using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3
{
    public sealed class InputHandlerDefault: 
        InputHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private readonly Vector3 _offset = new Vector3(0f, 0f, -0.1f);

        private Camera _camera;
        private Transform _cameraTransform;

        private ITile _tileFrom;
        private ITile _tileTo;

        private Transform _tileFromTransform;
        private Vector3 _startDragPosition;
        private Vector3 _dragPosition;
        private Vector3 _clampedDragPosition;

        public override void Init(GridViewer gridViewer, TileProvider tileProvider, ref IGrid grid)
        {
            GridViewer = gridViewer;
            TileProvider = tileProvider;
            Grid = grid;

            _camera = Camera.main.GetComponent<Camera>();
            _cameraTransform = _camera.GetComponent<Transform>();

            ActivateInput(true);

            Debug.Log("INIT DONE");
        }

        public override void AbortSwap()
        {
            _tileTo = null;

            if (_tileFrom == null) return;
            _tileFrom.Highlight(false);
            _tileFrom.SetPosition(FromIntPosition, GridSize);
            _tileFrom = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive) return;

            var tile = TileProvider.Get(
                _camera.ScreenToWorldPoint(eventData.position).
                ConvertFromVectorToInt(GridSize));

            if (tile == null) return;

            if (_tileFrom == null)
            {
                _tileFrom = tile;
                _tileFrom.Highlight(true);
                _tileFromTransform = _tileFrom.GetTransform();
                FromIntPosition = _tileFrom.GetIntPosition();

                _startDragPosition = FromIntPosition.ConvertFromIntToVector(GridSize);
            }
            else
            {
                if (tile == _tileFrom)
                {
                    AbortSwap();
                    return;
                }

                _tileTo = tile;
                ToIntPosition = _tileTo.GetIntPosition();
                TryApplySwap();
            }
           
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_tileFrom == null) return;

            _dragPosition = GetWorldRelativeDragPosition(eventData);

            if ((_dragPosition - _startDragPosition).magnitude < 1f)
            {
                _tileFromTransform.position = _dragPosition;
            }
            else
            {
                _clampedDragPosition = _startDragPosition + (_dragPosition - _startDragPosition).normalized;
                _tileFromTransform.position = _clampedDragPosition;
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_tileFrom == null) return;

            if (_tileTo == null)
            {
                var tile = TileProvider.Get(_tileFromTransform.position.
                     ConvertFromVectorToInt(GridSize));

                if (tile == null)
                {
                    return; 
                }

                _tileTo = tile;
                ToIntPosition = _tileTo.GetIntPosition();

                if (FromIntPosition != ToIntPosition)
                {
                    TryApplySwap();
                }
                else
                {
                    _tileFrom.SetPosition(FromIntPosition, GridSize);
                }

            }
        }

        private void TryApplySwap()
        {
            if (_tileFrom == null || _tileTo == null || _tileFrom == _tileTo 
                || (FromIntPosition.x != ToIntPosition.x && FromIntPosition.y != ToIntPosition.y)
                || Mathf.Abs(FromIntPosition.x - ToIntPosition.x) > 1 
                || Mathf.Abs(FromIntPosition.y - ToIntPosition.y) > 1)
            {
                AbortSwap();
                return;
            }

            GridViewer.Swap(FromIntPosition, ToIntPosition);

            if (_tileFrom != null)
            {
                _tileFrom.Highlight(false);
                _tileFrom = null;
                _tileFromTransform = null;
            }
           
            _tileTo = null;
        }

        private Vector3 GetWorldRelativeDragPosition(PointerEventData eventData)
        {
            return _camera.ScreenToWorldPoint(eventData.position)
                - _cameraTransform.position + _offset;
        }
    }

    
}
