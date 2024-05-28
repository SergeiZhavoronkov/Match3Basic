using System;
using System.Collections.Generic;
using System.Text;

namespace Match3 
{
    public sealed class GridDefault : IGrid
    {
        private const int _defaultGridSize = 7;

        private int[,] _grid;
        private int[,] _specialCells;
        private int _xSize;
        private int _ySize;

        private int _debugInfo_Rebuildings;

        public GridDefault(int xSize, int ySize)
        {
            Generate(Math.Abs(xSize), Math.Abs(ySize));
        }

        public GridDefault(int[,] grid)
        {
            if (grid == null)
            {
                Generate();
                return;
            }
            _grid = grid;
            _xSize = grid.GetLength(0);
            _ySize = grid.GetLength(1);
        }

        public int[,] Get()
        {
            if (_grid == null)
            {
                Generate(_defaultGridSize);
            }

            return _grid;
        }

        public void Set(int[,] grid)
        {
            if (grid == null) return;
            _grid = grid;
        }

        public bool TryGetMatches(out (int, int)[] matches)
        {
            var result = new List<(int, int)>();
            int lineLength;
            
            for (int x = 0; x < _xSize; x++)
            {
                lineLength = 1;
                for (int y = 1; y < _ySize; y++)
                {
                    if (_grid[x, y].Equals(_grid[x, y - 1]))
                    {
                        lineLength++;

                        if (y.Equals(_ySize - 1) && lineLength > 2)
                        {
                            for (int c = 0; c < lineLength; c++)
                            {
                                result.Add((x, y - c));
                            }
                            lineLength = 1;
                        }
                    }
                    else
                    {
                        if (lineLength > 2)
                        {
                            for (int c = 1; c <= lineLength; c++)
                            {
                                result.Add((x, y - c));
                            }
                        }
                        lineLength = 1;
                    }
                }
            }

            for (int y = 0; y < _ySize; y++)
            {
                lineLength = 1;
                for (int x = 1; x < _xSize; x++)
                {
                    if (_grid[x, y].Equals(_grid[x - 1, y]))
                    {
                        lineLength++;

                        if (x.Equals(_xSize - 1) && lineLength > 2)
                        {
                            for (int c = 0; c < lineLength; c++)
                            {
                                if (!result.Contains((x - c, y)))
                                {
                                    result.Add((x - c, y));
                                }
                            }
                            lineLength = 1;
                        }
                    }
                    else
                    {
                        if (lineLength > 2)
                        {
                            for (int c = 1; c <= lineLength; c++)
                            {
                                if (!result.Contains((x - c, y)))
                                {
                                    result.Add((x - c, y));
                                }
                            }
                        }
                        lineLength = 1;
                    }
                }
            }

            if (result.Count > 0)
            {     
                matches = result.ToArray();
                return true;
            }

            matches = null;
            return false;
        }

        public bool TrySwap((int, int) from, (int, int) to, out (int, int)[] matches)
        {
            if (from == to)
            {
                matches = null;
                return false;
            }

            var originFrom = _grid[from.Item1, from.Item2];
            var originTo = _grid[to.Item1, to.Item2];

            var temp = _grid[to.Item1, to.Item2];

            _grid[to.Item1, to.Item2] = _grid[from.Item1, from.Item2];
            _grid[from.Item1, from.Item2] = temp;

            if (TryGetMatches(out matches)) return true;

            _grid[from.Item1, from.Item2] = originFrom;
            _grid[to.Item1, to.Item2] = originTo;
            return false;
        }

        private void Generate(params int[] sizes)
        {
            if (sizes == null)
            {
                _xSize = _ySize = _defaultGridSize;
            }
            else if (sizes.Length == 1)
            {
                if (sizes[0] < 5)
                {
                    _xSize = _ySize = _defaultGridSize;
                }
                else
                {
                    _xSize = _ySize = sizes[0];
                }
            }
            else
            {
                _xSize = sizes[0];
                _ySize = sizes[1];
            }

            _grid = new int[_xSize, _ySize];
            _specialCells = new int[_xSize, _ySize];

            var rnd = new Random();

            for (int x = 0; x < _xSize; x++)
            {
                for (int y = 0; y < _ySize; y++)
                {
                    _grid[x, y] = rnd.Next(1, 6);
                }
            }

            while (TryGetMatches(out var matches))
            {
                for (int i = 1; i < matches.Length; i++)
                {
                    if (i % 2 != 0)
                    {
                        _grid[matches[i].Item1, matches[i].Item2] = rnd.Next(1, 6);
                    }
                }
                _debugInfo_Rebuildings++;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"REBUILDINGS: {_debugInfo_Rebuildings}");
            for (int y = 0; y < _ySize; y++)
            {
                if (y > 0)
                {
                    sb.AppendLine();
                }

                for (int x = 0; x < _xSize; x++)
                {
                    sb.Append($"[{_grid[x, y]}]");
                }
            }

            return sb.ToString();
        }


    }
}


