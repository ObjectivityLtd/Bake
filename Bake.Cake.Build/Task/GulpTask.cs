using Bake.API.Task;
using Cake.Core.IO;

namespace Bake.Cake.Build.Task
{
    public class GulpTask : ITask
    {
        public string Name => "Build " + this.NpmProjectName;

        public TaskType Type => TaskType.BuildFrontend;

        public DirectoryPath ProjectDir { get; }

        public string NpmProjectName { get; }

        public GulpTask(DirectoryPath projectDir, string npmProjectName)
        {
            this.ProjectDir = projectDir;
            this.NpmProjectName = npmProjectName ?? this.ProjectDir.GetDirectoryName();
        }
    }
}
