using System.Collections.Generic;

namespace Match3
{
    public interface IGridSolver<T>
    {
        public void SolveGridMatches(ref T gridData, (int x, int y)[] matches,
            out List<((int x, int y) from, (int x, int y) to)> moveData,
            out List<(int x, int y)> debetMoveData);
    }
}

