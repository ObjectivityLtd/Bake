using Bake.API.Task;
using Cake.Core.IO;

namespace Bake.Cake.Build.Task
{
    public class NpmTestsTask : ITask
    {
        public string Name => "Run tests " + this.NpmProjectName;

        public TaskType Type => TaskType.UnitTestFrontend;

        public DirectoryPath ProjectDir { get; }

        public string NpmProjectName { get; }

        public NpmTestsTask(DirectoryPath projectDir, string npmProjectName)
        {
            this.ProjectDir = projectDir;
            this.NpmProjectName = npmProjectName ?? this.ProjectDir.GetDirectoryName();
        }
    }
}
