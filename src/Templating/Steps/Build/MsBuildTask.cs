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

        public MsBuildTaskType TaskType { get; private set; }

        public FilePath SourceFile { get; private set; }

        public string SolutionName { get; private set; }

        public MsBuildTask(MsBuildTaskType taskType, FilePath sourceFile, string solutionName)
        {
            this.TaskType = taskType;
            this.SourceFile = sourceFile;
            this.SolutionName = solutionName;
        }

    }
}
