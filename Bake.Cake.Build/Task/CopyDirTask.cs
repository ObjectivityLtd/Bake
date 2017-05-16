using Bake.API.Task;
using Cake.Core.IO;

namespace Bake.Cake.Build.Task
{
    public class CopyDirTask : ITask
    {
        public string Name => "Copy " + this.SourceDir.GetDirectoryName();

        public TaskType Type => TaskType.BuildBackend;

        public DirectoryPath SourceDir { get; }

        public CopyDirTask(DirectoryPath sourceDir)
        {
            this.SourceDir = SourceDir;
        }

    }
}
