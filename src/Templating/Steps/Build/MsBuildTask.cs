using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class MsBuildTask : IScriptTask
    {

        public enum MsBuildTaskType
        {
            WebApplication,
            ConsoleApplication
        }

        public string Name => "Build " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public ScriptTaskType Type => ScriptTaskType.BuildBackend;

        public MsBuildTaskType TaskType { get; }

        public FilePath SourceFile { get; }

        public string ProjectName { get; }

        public MsBuildTask(MsBuildTaskType taskType, FilePath sourceFile, string projectName)
        {
            this.TaskType = taskType;
            this.SourceFile = sourceFile;
            this.ProjectName = projectName;
        }

    }
}
