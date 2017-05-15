using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class BuildTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return !solutionInfo.BuildSolution
                   && solutionInfo.Projects.Any(projectInfo => projectInfo.IsUnitTestProject());
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            return new List<IScriptTask>
            {
                new MsTestTestsTask(solutionInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }

        
    }
}
