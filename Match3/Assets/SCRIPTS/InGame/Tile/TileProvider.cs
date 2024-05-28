using UnityEngine;

namespace Match3
{
    public sealed class TileProvider
    {
        private readonly (int x, int y) _defaultPosition = (0, 15);
        private readonly (int x, int y) _onDebetPosition = (0, 14);
        private readonly (int x, int y) _defaultGridSize = (7, 7);

        private ITile[] _tiles;

        public TileProvider(GameObject tilePrefab, int count, Transform parent)
        {
            _tiles = new ITile[count];

            for (int i = 0; i < count; i++)
            {
                _tiles[i] = GameObject.Instantiate(tilePrefab, parent).GetComponent<ITile>() as ITile;
                _tiles[i].Init();
                _tiles[i].SetPosition(_defaultPosition, _defaultGridSize);
            }
        }

        public ITile Get((int x, int y) intPosition)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i].GetIntPosition() == intPosition)
                {
                    return _tiles[i];
                }
            }
            return null;
        }

        public ITile GetFreeTile()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i].GetIntPosition() == _defaultPosition)
                {
                    _tiles[i].SetPosition(_onDebetPosition, _defaultGridSize);
                    return _tiles[i];
                }
            }
            return null;
        }

        public void RemoveTileFromGrid(ITile tile)
        {
            tile.SetPosition(_defaultPosition, _defaultGridSize);
        }
    }
}
