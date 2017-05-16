using Bake.API.Task;
using Cake.Core.IO;

namespace Bake.Cake.Build.Task
{
    public class EntityFrameworkMigrationsTask : ITask
    {
        public string Name => "Build " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public TaskType Type => TaskType.BuildBackend;

        public FilePath SourceFile { get; }

        public string ProjectName { get; }

        public FilePath EntityFrameworkDllFilePath { get; }

        public FilePath EntityFrameworkMigrateFilePath { get; }

        public EntityFrameworkMigrationsTask(FilePath sourceFile, string projectName, 
            FilePath entityFrameworkDllFilePath, FilePath entityFrameworkMigrateFilePath)
        {
            this.SourceFile = sourceFile;
            this.ProjectName = projectName;
            this.EntityFrameworkDllFilePath = entityFrameworkDllFilePath;
            this.EntityFrameworkMigrateFilePath = entityFrameworkMigrateFilePath;
        }
    }
}
