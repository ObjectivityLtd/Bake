using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class WebDriverTestsTask : IScriptTask
    {
        public string Name => "Build " + this.SourceFile.GetFilenameWithoutExtension().FullPath;

        public ScriptTaskType Type => ScriptTaskType.BuildIntegrationTests;

        public FilePath SourceFile { get; }

        public string ProjectName { get; }

        public bool RestoreNuget { get; }

        public WebDriverTestsTask(FilePath sourceFile, string projectName, bool restoreNuget)
        {
            this.SourceFile = sourceFile;
            this.ProjectName = projectName;
            this.RestoreNuget = restoreNuget;
        }

    }
}
