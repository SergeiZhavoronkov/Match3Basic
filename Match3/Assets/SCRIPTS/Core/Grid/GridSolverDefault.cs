using System;
using System.Collections.Generic;

namespace Match3 
{
    public sealed class GridSolverDefault : IGridSolver<int[,]>
    {
        public void SolveGridMatches(ref int[,] grid, (int x, int y)[] matches, 
            out List<((int x, int y) from, (int x, int y) to)> moveData,
            out List<(int x, int y)> debetMoveData)
        {
            for (int i = 0; i < matches.Length; i++)
            {
                grid[matches[i].x, matches[i].y] = 0;
            }

            var xSize = grid.GetLength(0);
            var ySize = grid.GetLength(1);

            moveData = new List<((int x, int y) from, (int x, int y) to)>();
            debetMoveData = new List<(int x, int y)>();
            var rnd = new Random();
            int offset;

            for (int x = 0; x < xSize; x++)
            {
                offset = 0;
                for (int y = 0; y < ySize; y++)
                {
                    if (grid[x, y].Equals(0))
                    {
                        offset++;
                    }
                    else if (grid[x, y] > 0 && y > 0 && offset > 0)
                    {
                        moveData.Add(((x, y), (x, y - offset)));
                        grid[x, y - offset] = grid[x, y];
                        grid[x, y] = 0;
                    }
                }

                for (int y = 0; y < ySize; y++)
                {
                    if (grid[x, y].Equals(0))
                    {
                        grid[x, y] = rnd.Next(1, 6);
                        debetMoveData.Add((x, y));
                    }
                }
            }
        }
    }
}


