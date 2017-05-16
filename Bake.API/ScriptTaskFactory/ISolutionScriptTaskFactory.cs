using System.Collections.Generic;
using Bake.API.Task;

namespace Bake.API.ScriptTaskFactory
{
    public interface ISolutionScriptTaskFactory
    {
        int Order { get; }

        IEnumerable<ITask> Create(SolutionInfo solutionInfo);

        bool IsApplicable(SolutionInfo projectsolutionInfoInfo);
    }
}