using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class ConsoleApplicationFactory : AbstractScriptTaskFactory
    {
        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsExecutableApplication();
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            var restoreNuget = projectInfo.SolutionFilePath == null;
            return new List<IScriptTask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.ConsoleApplication,
                    sourceFile: projectInfo.Project.Path,
                    projectName: projectInfo.Project.Name,
                    createPackage: true,
                    restoreNuget: restoreNuget)
            };
        }
    }
}
