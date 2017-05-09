using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class GulpTask : IScriptTask
    {
        public string Name => "Build " + this.ProjectDir.GetDirectoryName();

        public DirectoryPath ProjectDir { get; }

        public GulpTask(DirectoryPath projectDir)
        {
            this.ProjectDir = projectDir;
        }
    }
}
