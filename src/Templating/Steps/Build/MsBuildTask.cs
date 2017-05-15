using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class MsBuildTask : IScriptTask
    {

        public enum MsBuildTaskType
        {
            Solution,
            WebApplication,
            ConsoleApplication
        }

        public string Name => "Build " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public ScriptTaskType Type => ScriptTaskType.BuildBackend;

        public MsBuildTaskType TaskType { get; }

        public FilePath SourceFile { get; }

        public string ProjectName { get; }

        public bool RestoreNuget { get; }

        public bool CreatePackage { get; }

        public MsBuildTask(MsBuildTaskType taskType, FilePath sourceFile, string projectName, bool createPackage, bool restoreNuget)
        {
            this.TaskType = taskType;
            this.SourceFile = sourceFile;
            this.ProjectName = projectName;
            this.RestoreNuget = restoreNuget;
            this.CreatePackage = createPackage;
        }

    }
}
