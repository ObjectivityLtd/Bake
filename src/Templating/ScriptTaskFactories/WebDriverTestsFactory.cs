using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class WebDriverTestsFactory : AbstractScriptTaskFactory
    {
        public override int ParsingOrder => 0;

        public override bool IsTerminating => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsUnitTestProject() && projectInfo.FindReference("WebDriver") != null;
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new WebDriverTestsTask(projectInfo.Project.Path, projectInfo.Project.Name)
            };
        }
    }
}
