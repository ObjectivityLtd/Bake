using System.Collections.Generic;
using Bake.API.ScriptTaskFactory;
using Bake.API.Task;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public abstract class AbstractSolutionScriptTaskFactory : ISolutionScriptTaskFactory
    {
        public virtual int Order => 10;

        public abstract bool IsApplicable(SolutionInfo solutionInfo);

        public abstract IEnumerable<ITask> Create(SolutionInfo projsolutionInfoectInfo);
    }
}
