using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class EntityFrameworkMigrationsTask : IScriptTask
    {
        public string Name => "Build " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public ScriptTaskType Type => ScriptTaskType.BuildBackend;

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
