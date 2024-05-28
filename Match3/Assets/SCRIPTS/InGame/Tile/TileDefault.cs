using UnityEngine;

namespace Match3
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Collider))]
    public sealed class TileDefault : MonoBehaviour, ITile
    {
        private (int x, int y) _intPosition;
        private (int x, int y) _gridSize;
        private int _id;

        private Renderer _renderer;
        private Transform _transform;

        [SerializeField] private MaterialPack_SO _materials;
        [Header("DEBUG:")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        private Vector3 _startDragPosition;
        private Vector3 _dragPosition;

        public void Init()
        {
            _renderer = GetComponent<Renderer>();
            _transform = GetComponent<Transform>();
        }

        public void SetID(int id)
        {
            if (_materials.TryGetMaterial(id - 1, out var material)) 
            {
                _id = id;
                _renderer.material = material;
            }
        }

        public int GetID()
        {
            return _id;
        }

        public Transform GetTransform()
        {
            return _transform;
        }

        public (int x, int y) GetIntPosition()
        {
            return _intPosition;
        }

        public void SetPosition((int x, int y) position, (int x, int y) gridSize)
        {
            _transform.position = position.ConvertFromIntToVector(gridSize);
            _gridSize = gridSize;

            UpdateIntPosition();
        }

        public void UpdateIntPosition()
        {
            _intPosition = _transform.position.ConvertFromVectorToInt(_gridSize);
            _x = _intPosition.x;
            _y = _intPosition.y;
        }

        public void Highlight(bool highlight)
        {
            if (highlight)
            {
                _transform.position +=  0.1f * Vector3.back;
                _transform.localScale = 1.25f * Vector3.one;
            }
            else
            {
                _transform.position += 0.1f * Vector3.forward;
                _transform.localScale = Vector3.one;
            }
        }
    }
}
