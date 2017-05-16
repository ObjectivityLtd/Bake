using Bake.API.Task;

namespace Bake.Cake.Build.Task
{
    public class RestoreNugetTask : ITask
    {
        public string Name => "Restore nuget " + this.SolutionName;

        public TaskType Type => TaskType.BuildBackend;

        public string SolutionName { get; }

        public RestoreNugetTask(string solutionName)
        {
            this.SolutionName = solutionName;
        }
    }
}
