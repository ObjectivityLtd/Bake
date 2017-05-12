using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class WebApplicationFactory : IScriptTaskFactory
    {
        public int Order => 10;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsWebApplication();
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new MsBuildTask(MsBuildTask.MsBuildTaskType.WebApplication, projectInfo.Project.Path, projectInfo.Project.Name)
            };
        }
    }
}
