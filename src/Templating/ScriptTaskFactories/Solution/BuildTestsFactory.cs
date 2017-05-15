using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class BuildTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 20;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return !solutionInfo.BuildSolution
                   && solutionInfo.Projects.Any(projectInfo => projectInfo.IsUnitTestProject());
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            var testProjects = solutionInfo.Projects.Where(projectInfo => projectInfo.IsUnitTestProject() && !projectInfo.IsWebDriverProject())
                .Select(projectInfo => projectInfo.Project.Path);
            return new List<IScriptTask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.UnitTests,
                    sourceFiles: testProjects,
                    projectName: "")
            };
        }

        
    }
}
