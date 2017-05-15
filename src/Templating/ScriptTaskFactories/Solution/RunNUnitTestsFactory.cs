using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class NUnitTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 21;

        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return solutionInfo.Projects.Any(projectInfo => projectInfo.FindReference("nunit") != null);
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            return new List<IScriptTask>
            {
                new NUnitTestsTask(solutionInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }

        
    }
}
