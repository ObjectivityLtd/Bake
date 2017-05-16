using Bake.API.Task;

namespace Bake.Cake.Build.Task
{
    public abstract class AbstractBackendTestsTask : ITask
    {
        public string Name => "Run tests " + this.SolutionName;

        public TaskType Type => TaskType.UnitTestBackend;

        public string SolutionName { get; }

        public AbstractBackendTestsTask(string solutionName)
        {
            this.SolutionName = solutionName;
        }
    }
}
