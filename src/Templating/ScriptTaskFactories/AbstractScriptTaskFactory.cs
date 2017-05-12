using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public abstract class AbstractScriptTaskFactory : IScriptTaskFactory
    {
        public virtual int ParsingOrder => 10;

        public virtual bool IsTerminating => false;

        public virtual bool IsSolutionLevel => false;

        public abstract bool IsApplicable(ProjectInfo projectInfo);

        public abstract IEnumerable<IScriptTask> Create(ProjectInfo projectInfo);
    }
}
