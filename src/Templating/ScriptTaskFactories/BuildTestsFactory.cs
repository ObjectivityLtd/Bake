using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class BuildTestsFactory : AbstractScriptTaskFactory
    {
        public override int ParsingOrder => 0;

        public override bool IsTerminating => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsUnitTestProject())
            {
                return false;
            }
            return projectInfo.FindReference("WebDriver") != null;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            var restoreNuget = projectInfo.SolutionFilePath == null;
            return new List<IScriptTask>
            {
                new WebDriverTestsTask(projectInfo.Project.Path, projectInfo.Project.Name, restoreNuget)
            };
        }
    }
}
