using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class CopyDirTask : IScriptTask
    {
        public string Name => "Copy " + this.SourceDir.GetDirectoryName();

        public ScriptTaskType Type => ScriptTaskType.BUILD_BACKEND;

        public DirectoryPath SourceDir { get; }

        public CopyDirTask(DirectoryPath sourceDir)
        {
            this.SourceDir = SourceDir;
        }

    }
}
