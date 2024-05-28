namespace Match3 
{
    public interface IGrid
    {
        public int[,] Get();

        public void Set(int[,] grid = null);

        public bool TryGetMatches(out (int, int)[] matches);

        public bool TrySwap((int, int) from, (int, int) to, out (int, int)[] matches);
    }
}


