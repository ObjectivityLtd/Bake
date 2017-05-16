using Bake.API.Task;

namespace Bake.Cake.Build.Task
{
    public class CleanTask : ITask
    {
        public string Name => "Clean";

        public TaskType Type => TaskType.BuildBackend;
    }
}
