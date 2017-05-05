using Cake.CD.Scripting;

namespace Cake.CD.Templating.Build
{
    public class MsBuildTask : IScriptTask
    {

        public enum MsBuildTaskType
        {
            WEB_APPLICATION,
            CONSOLE_APPLICATION
        }

        public MsBuildTaskType TaskType { get; private set; }

        public string SourceFile { get; private set; }

        public string DestinationFile { get; private set; }

        public MsBuildTask(MsBuildTaskType TaskType, string SourceFile, string DestinationFile)
        {
            this.TaskType = TaskType;
            this.SourceFile = SourceFile;
            this.DestinationFile = DestinationFile;
        }

    }
}
