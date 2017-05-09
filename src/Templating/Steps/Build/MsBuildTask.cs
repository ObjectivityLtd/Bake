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

        public MsBuildTaskType TaskType { get; }

        public FilePath SourceFile { get; }

        public string SolutionName { get; }

        public MsBuildTask(MsBuildTaskType taskType, FilePath sourceFile, string solutionName)
        {
            this.TaskType = taskType;
            this.SourceFile = sourceFile;
            this.SolutionName = solutionName;
        }

    }
}
