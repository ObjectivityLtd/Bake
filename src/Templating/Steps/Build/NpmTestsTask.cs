using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class NpmTestsTask : IScriptTask
    {
        public string Name => "Run tests " + this.NpmProjectName;

        public ScriptTaskType Type => ScriptTaskType.UnitTestFrontend;

        public DirectoryPath ProjectDir { get; }

        public string NpmProjectName { get; }

        public NpmTestsTask(DirectoryPath projectDir, string npmProjectName)
        {
            this.ProjectDir = projectDir;
            this.NpmProjectName = npmProjectName ?? this.ProjectDir.GetDirectoryName();
        }
    }
}
