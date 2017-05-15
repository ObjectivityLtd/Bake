using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Project
{
    public abstract class AbstractProjectScriptTaskFactory : IProjectScriptTaskFactory
    {
        public virtual int Order => 10;

        public abstract bool IsApplicable(ProjectInfo projectInfo);

        public abstract IEnumerable<IScriptTask> Create(ProjectInfo projectInfo);
    }
}
