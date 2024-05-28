namespace Match3
{
    public interface ITaskHandler<T>
    {
        public void Run(T data);

        public void InformTaskCompleted();

        public bool AllTasksCompleted();
    }
}
