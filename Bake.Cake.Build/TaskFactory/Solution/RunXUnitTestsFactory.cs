using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using System.Collections.Generic;
using System.Linq;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public class RunXUnitTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 21;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return solutionInfo.Projects.Any(projectInfo => projectInfo.FindReference("xunit") != null && !projectInfo.IsWebDriverProject());
        }

        public override IEnumerable<ITask> Create(SolutionInfo solutionInfo)
        {
            return new List<ITask>
            {
                new XUnitTestsTask(solutionInfo.Name)
            };
        }  
    }
}
