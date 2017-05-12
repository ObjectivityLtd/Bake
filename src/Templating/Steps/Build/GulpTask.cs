using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class GulpTask : IScriptTask
    {
        public string Name => "Build " + this.ProjectDir.GetDirectoryName();

        public ScriptTaskType Type => ScriptTaskType.BUILD_FRONTEND;

        public DirectoryPath ProjectDir { get; }

        public GulpTask(DirectoryPath projectDir)
        {
            this.ProjectDir = projectDir;
        }
    }
}
