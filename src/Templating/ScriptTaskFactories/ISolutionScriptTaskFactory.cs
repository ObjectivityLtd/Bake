using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public interface ISolutionScriptTaskFactory
    {
        int Order { get; }

        IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo);

        bool IsApplicable(SolutionInfo projectsolutionInfoInfo);
    }
}