using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using System.Collections.Generic;
using System.Linq;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public class BuildTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 20;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return !solutionInfo.BuildSolution
                   && solutionInfo.Projects.Any(projectInfo => projectInfo.IsUnitTestProject());
        }

        public override IEnumerable<ITask> Create(SolutionInfo solutionInfo)
        {
            var testProjects = solutionInfo.Projects.Where(projectInfo => projectInfo.IsUnitTestProject() && !projectInfo.IsWebDriverProject())
                .Select(projectInfo => projectInfo.Path);
            return new List<ITask>
            {
                new MsBuildTask(
                    taskType: MsBuildTask.MsBuildTaskType.UnitTests,
                    sourceFiles: testProjects,
                    projectName: "")
            };
        }

        
    }
}
