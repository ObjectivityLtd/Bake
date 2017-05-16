namespace Bake.API.Task
{
    public interface ITask
    {
        string Name { get; }

        TaskType Type { get; }

    }
}
