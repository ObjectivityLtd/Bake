using Bake.API.Task;
using Cake.Core.IO;
using System.Collections.Generic;

namespace Bake.Cake.Build.Task
{
    public class MsBuildTask : ITask
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

        public TaskType Type => GetTaskType();
        

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

        private TaskType GetTaskType()
        {
            switch (TaskType)
            {
                case MsBuildTaskType.UiTests: return API.Task.TaskType.BuildIntegrationTests;
                case MsBuildTaskType.UnitTests: return API.Task.TaskType.BuildUnitTests;
                default: return API.Task.TaskType.BuildBackend;
            }
        }

    }
}
