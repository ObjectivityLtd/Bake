using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.CD.Templating.ScriptTaskFactories.Project
{
    public class WebApplicationFactory : AbstractProjectScriptTaskFactory
    {
        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsWebApplication();
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.WebApplication,
                    sourceFiles: new List<FilePath> { projectInfo.Project.Path },
                    projectName: projectInfo.Project.Name)
            };
        }
    }
}
