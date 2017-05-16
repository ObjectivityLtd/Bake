using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Cake.Core.IO;
using System.Collections.Generic;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Project
{
    public class WebDriverTestsFactory : AbstractProjectScriptTaskFactory
    {
        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsUnitTestProject() && projectInfo.IsWebDriverProject();
        }

        public override IEnumerable<ITask> Create(ProjectInfo projectInfo)
        {
            return new List<ITask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.UiTests,
                    sourceFiles: new List<FilePath> { projectInfo.Project.Path },
                    projectName: projectInfo.Project.Name)
            };
        }
    }
}
