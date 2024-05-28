using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Match3 
{
    [DisallowMultipleComponent]
    public sealed class GridViewer : MonoBehaviour
    {
        private IGrid _grid;
        private IGridSolver<int[,]> _solver;
       
        private TileProvider _tileProvider;
        private ITaskHandler<((int, int), (int, int))> _tileSwapper;
        private ITaskHandler<(int, int)[]> _tileRemover;
        private ITaskHandler<List<((int, int), (int, int))>> _tileMover;
        private ITaskHandler<(List<(int x, int y)>, int[,] grid)> _tileDebetMover;

        private (int, int)[] _matches;

        [Header("INPUT HANDLER")]
        [SerializeField] private InputHandler _inputHandler;
        [Header("TILE PREFAB:")]
        [SerializeField] private GameObject _tilePrefab;

        [Header("GRID SIZE")]
        [SerializeField] private int _xSize;
        [SerializeField] private int _ySize;

        private void Start()
        {
            _solver = new GridSolverDefault();
            _tileProvider = new TileProvider(_tilePrefab, _xSize * _ySize, GetComponent<Transform>());
            _tileSwapper = new TileSwapperDefault(_tileProvider, _xSize, _ySize);
            _tileRemover = new TileRemoverDefault(_tileProvider);
            _tileMover = new TileMoverDefault(_tileProvider, _xSize, _ySize);
            _tileDebetMover = new TileDebetMoverDefault(_tileProvider, _xSize, _ySize);

            GenerateGrid();

            _inputHandler.Init(this, _tileProvider, ref _grid);
        }

        public void GenerateGrid()
        {
            _grid = new GridDefault(_xSize, _ySize);

            var grid = _grid.Get();

            for (int x = 0; x < _xSize; x++) 
            {
                for (int y = 0; y < _ySize; y++) 
                {
                    var tile = _tileProvider.GetFreeTile();
                    tile.SetPosition((x, y), (_xSize, _ySize));
                    tile.SetID(grid[x, y]);
                }
            }

            Debug.Log(_grid.ToString());
        }

        public async void Swap((int x, int y) from, (int x, int y) to)
        {
            if (_grid.TrySwap(from, to, out var matches))
            {
                _inputHandler.ActivateInput(false);

                var grid = _grid.Get();
                _matches = matches;

                _tileSwapper.Run((from, to));
                await UniTask.WaitUntil(() => _tileSwapper.AllTasksCompleted());

                while (_matches != null)
                {
                    grid = _grid.Get();
                    _solver.SolveGridMatches(ref grid, _matches,
                        out var moveData, out var debetMoveData);

                    _tileRemover.Run(_matches);
                    await UniTask.WaitUntil(() => _tileRemover.AllTasksCompleted());

                    _tileMover.Run(moveData);
                    _tileDebetMover.Run((debetMoveData, grid));
                    await UniTask.WaitUntil(() => _tileDebetMover.AllTasksCompleted());

                    _grid.Set(grid);
                    _grid.TryGetMatches(out _matches);
                }
            }
            else
            {
                _inputHandler.AbortSwap();
            }
            _inputHandler.ActivateInput(true);

        }
  
        private void LogIntDataArray(string header, (int, int)[] data)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{header} ({data.Length}): ");
            for (int i = 0; i < data.Length; i++) 
            {
                sb.Append($"{data[i].Item1}x{data[i].Item2}; ");
            }
            Debug.Log(sb.ToString());
        }

        /*private void OnDrawGizmosSelected()
        {
            if (_grid == null) return;

            var grid = _grid.Get();
            var xSize = grid.GetLength(0);
            var ySize = grid.GetLength(1);

            var offsetX = xSize / -2f + 0.5f;
            var offsetY = ySize / -2f + 0.5f;

            var violet = new Color(0.5f, 0f, 1f);

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (grid[x, y] == 0) Gizmos.color = Color.gray;
                    else if (grid[x, y] == 1) Gizmos.color = Color.red;
                    else if (grid[x, y] == 2) Gizmos.color = Color.green;
                    else if (grid[x, y] == 3) Gizmos.color = Color.blue;
                    else if (grid[x, y] == 4) Gizmos.color = Color.yellow;
                    else Gizmos.color = violet;

                    Gizmos.DrawCube(new Vector3(x + offsetX, y + offsetY, 1f), 0.9f * Vector3.one);
                }
            }
        }  */
    }
}


