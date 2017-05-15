using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public abstract class AbstractSolutionScriptTaskFactory : ISolutionScriptTaskFactory
    {
        public virtual int ParsingOrder => 10;

        public abstract bool IsApplicable(SolutionInfo solutionInfo);

        public abstract IEnumerable<IScriptTask> Create(SolutionInfo projsolutionInfoectInfo);
    }
}
