using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class RestoreNuGetTask : IScriptTask
    {
        public string Name => "RestoreNuGet " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public FilePath SourceFile { get; }

        public RestoreNuGetTask(FilePath sourceFile)
        {
            this.SourceFile = sourceFile;
        }

    }
}
