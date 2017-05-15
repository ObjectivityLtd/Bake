using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class MsBuildTask : IScriptTask
    {

        public enum MsBuildTaskType
        {
            Solution,
            WebApplication,
            ConsoleApplication,
            UnitTests,
            UiTests
        }

        public string Name => "Build " + (ProjectName != "" ? ProjectName : TaskType.ToString());

        public ScriptTaskType Type => GetScriptTaskType();
        

        public MsBuildTaskType TaskType { get; }

        public IEnumerable<FilePath> SourceFiles { get; }

        public string ProjectName { get; }

        public bool CreateMsDeployPackage => TaskType == MsBuildTaskType.WebApplication;

        public MsBuildTask(MsBuildTaskType taskType, IEnumerable<FilePath> sourceFiles, string projectName)
        {
            this.TaskType = taskType;
            this.SourceFiles = sourceFiles;
            this.ProjectName = projectName;
        }

        private ScriptTaskType GetScriptTaskType()
        {
            switch (TaskType)
            {
                case MsBuildTaskType.UiTests: return ScriptTaskType.BuildIntegrationTests;
                case MsBuildTaskType.UnitTests: return ScriptTaskType.BuildUnitTests;
                default: return ScriptTaskType.BuildBackend;
            }
        }

    }
}
