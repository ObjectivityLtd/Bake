using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class WebDriverTestsFactory : IScriptTaskFactory
    {
        public int Order => 0;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsUnitTestProject() && projectInfo.FindReference("WebDriver") != null;
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new WebDriverTestsTask(projectInfo.Project.Path, projectInfo.Project.Name)
            };
        }
    }
}
