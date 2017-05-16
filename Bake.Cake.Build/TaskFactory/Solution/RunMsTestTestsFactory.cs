using Bake.API.MsBuild;
using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Bake.Cake.Build.Task;
using System.Collections.Generic;
using System.Linq;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public class RunMsTestTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 21;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return solutionInfo.Projects.Any(projectInfo =>
                projectInfo.ParserResult != null
                && projectInfo.ParserResult.IsTestProjectType(projectInfo.Path.FullPath)
                && projectInfo.ParserResult.IsMsTestProject(projectInfo.Path.FullPath)
                && projectInfo.FindReference("nunit") == null
                && projectInfo.FindReference("xunit") == null
                && !projectInfo.IsWebDriverProject());
        }

        public override IEnumerable<ITask> Create(SolutionInfo solutionInfo)
        {
            return new List<ITask>
            {
                new MsTestTestsTask(solutionInfo.Name)
            };
        }

        
    }
}
