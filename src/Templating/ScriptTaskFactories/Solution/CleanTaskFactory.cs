using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class CleanTaskFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 0;

        public override bool IsApplicable(SolutionInfo projectInfo)
        {
            return true;
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            return new List<IScriptTask>
            {
                new CleanTask()
            };
        }

        
    }
}
