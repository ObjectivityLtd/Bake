using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Project
{
    public class WebDriverTestsFactory : AbstractProjectScriptTaskFactory
    {
        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsUnitTestProject() && projectInfo.FindReference("WebDriver") != null;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            var restoreNuget = projectInfo.SolutionInfo == null;
            return new List<IScriptTask>
            {
                new WebDriverTestsTask(projectInfo.Project.Path, projectInfo.Project.Name, restoreNuget)
            };
        }
    }
}
