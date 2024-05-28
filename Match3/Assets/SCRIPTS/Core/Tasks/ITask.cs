namespace Match3
{
    public interface ITask<T> where T : struct
    {
        public void Run(T runData);

        public bool IsCompleted();
    }
}
